using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

namespace Randy.Core
{
    public class Serializer
    {

        public T DeserilizeBinary<T>(MemoryStream data)
        {

            var formater = new BinaryFormatter();
            var result = formater.Deserialize(data);
            return (T)result;
        }

        public MemoryStream SerilizeAsBinary(object data)
        {
            MemoryStream ms;
            using (ms = new MemoryStream())
            {
                var formater = new BinaryFormatter();
                formater.Serialize(ms, data);
            }
            return ms;
        }

        public string SerilizeAsXml<T>(T data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, data, Encoding.UTF8);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        public T DeserilizeXml<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("s");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }

        }

        public string SerilizeAsJson<T>(T data)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, data);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public T DeserilizeJson<T>(string data)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }


    }
}
