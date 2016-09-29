using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCommons
{
    public class Logger
    {
        static string LogFileRootPath = @"C:\SharpMemberLogs\"; // must end with '\'
        static private object Locker = new object();
        static private bool hasInitialized = false;
        const string timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        static public void Init_Internal(bool addConsoleTraceListener)
        {
            System.Diagnostics.Trace.AutoFlush = true;

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new System.Diagnostics.DefaultTraceListener()); // system default
            if (addConsoleTraceListener)
            {
                Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            }

            //string logFile = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
            string logFile = string.Format("{0}sclog-{1}{2}", LogFileRootPath, DateTime.Now.ToString("yyyy-MM-dd"), ".log");
            Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(logFile));

            hasInitialized = true;

            Logger.WriteInfo(string.Format("Logger initialized, current log file: {0}", logFile));
        }

        /// <summary>
        /// Initialize the log file and timer
        /// </summary>
        static public void Init(bool addConsoleTraceListener = false)
        {
            lock(Locker) {
                if (!hasInitialized)
                {
                    Init_Internal(addConsoleTraceListener);
                    
                    var timer = new System.Timers.Timer(86400000); // 24 hours
                    timer.Elapsed += (s, e) => {
                        Logger.WriteInfo(string.Format("Log initialization timer event triggered. Interval: ", timer.Interval));
                        Logger.Init_Internal(addConsoleTraceListener);
                    };
                    timer.Enabled = true;
                }
            }
        }

        static public void WriteException(Exception ex, string additionalMsg = "")
        {
            lock (Locker)
            {
                WriteError(ex.Message + " | " + additionalMsg);

                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    Trace.WriteLine("InnerException: " + innerException.Message);
                    innerException = innerException.InnerException;
                }

                Trace.WriteLine(ex.StackTrace);
            }
        }

        static public void WriteError(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine(String.Format("{0} | {1} | {2}", DateTime.Now.ToString(timeFormat), "Error  ", message));
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        static public void WriteWarning(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine(String.Format("{0} | {1} | {2}", DateTime.Now.ToString(timeFormat), "Warning", message));
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        static public void WriteInfo(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine(String.Format("{0} | {1} | {2}", DateTime.Now.ToString(timeFormat), "Info   ", message));
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }
        
        static public void WriteTrace(string format, params object[] args)
        {
#if DEBUG
            lock (Locker)
            {
                string message = string.Format(format, args);
                Trace.WriteLine(String.Format("{0} | {1} | {2}", DateTime.Now.ToString(timeFormat), "Trace  ", message));
            }
#endif
        }
    }
}
