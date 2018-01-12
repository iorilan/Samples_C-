using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Imps.Services.CommonV4;

namespace ServerToolServer.Utility
{
    public class XMLSerializerEx
    {

        /// <summary>
        /// 对象转换成XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SaveXmlFromObj<T>(T obj)
        {
            if (obj == null) return null;
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            
            MemoryStream stream = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(stream, Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;
            try
            {
                serializer.Serialize(stream, obj,namespaces);
            }
            catch { return null; }

            stream.Position = 0;
            string returnStr = string.Empty;
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    returnStr += line;
                }
            }
            return returnStr;
        }


        public static T LoadObjFromXML<T>(Stream s)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            try
            {
                return ((T)serializer.Deserialize(s));
            }
            catch { return default(T); }
        }
        public static T LoadObjFromXML<T>(Stream s,string nameSpace)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T),nameSpace);
            try
            {
                return ((T)serializer.Deserialize(s));
            }
            catch { return default(T); }
        }

        /// <summary>
        /// XML反序列化到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T LoadObjFromXML<T>(string data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    sw.Write(data);
                    sw.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return LoadObjFromXML<T>(stream);

                }
            }
        }
        public static T LoadObjFromXML<T>(string data,string nameSpace)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    sw.Write(data);
                    sw.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return LoadObjFromXML<T>(stream,nameSpace);

                }
            }
        }
        
    }
}
