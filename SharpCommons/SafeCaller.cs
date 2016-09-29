using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCommons
{
    public class SafeCall
    {
        static public void ExecuteWithError(Action fn)
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        static public void ExecuteWithWarning(Action fn)
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                Logger.WriteWarning(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }
    }
}
