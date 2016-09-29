using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharpCommons
{
    public class GenericXMLSerializer<T>
    {
        public void Save(T obj, string filepath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                using (Stream file = new FileStream(filepath, FileMode.Create))
                {
                    XmlSerializer writer = new XmlSerializer(typeof(T));
                    writer.Serialize(file, obj);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        public T Load(string filepath)
        {
            T retval = default(T); // ---> NOTICE

            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(filepath))
                    {
                        XmlSerializer reader = new XmlSerializer(typeof(T)); // ---> NOTICE: will throw an first-chance exception, see: "error-debug. c#. FileNotFoundException from XmlSerializer constructor.mcn"
                        retval = (T)reader.Deserialize(file);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return retval;
        }
    }
}
