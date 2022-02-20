using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SharedClasses
{
    public static class XmlSerializerFactoryNoThrow
    {
        public static Dictionary<Type, XmlSerializer> _cache = new Dictionary<Type, XmlSerializer>();

        private static object SyncRootCache = new object();

        /// <summary>
        /// //the constructor XmlSerializer.FromTypes does not throw exception, but it is said that it causes memory leaks
        /// http://stackoverflow.com/questions/1127431/xmlserializer-giving-filenotfoundexception-at-constructor
        /// That is why I use dictionary to cache the serializers my self.
        /// </summary>
        public static XmlSerializer Create(Type type)
        {
            XmlSerializer serializer;

            lock (SyncRootCache)
            {
                if (_cache.TryGetValue(type, out serializer))
                    return serializer;
            }

            lock (type) //multiple variable of type of one type is same instance
            {
                //constructor XmlSerializer.FromTypes does not throw the first chance exception           
                serializer = XmlSerializer.FromTypes(new[] { type })[0];
                //serializer = XmlSerializerFactoryNoThrow.Create(type);
            }

            lock (SyncRootCache)
            {
                _cache[type] = serializer;
            }
            return serializer;
        }
    }
    public class XMLSerializer
    {
        /// <summary>
        /// populate a class with xml data 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="input">xml data</param>
        /// <returns>Object Type</returns>
        public static T Deserialize<T>(string input) where T : class
        {

            //System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
            //
            System.Xml.Serialization.XmlSerializer ser = XmlSerializerFactoryNoThrow.Create(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        /// <summary>
        /// convert object to xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ObjectToSerialize"></param>
        /// <returns></returns>
        public static string Serialize<T>(T ObjectToSerialize)
        {
            //XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            System.Xml.Serialization.XmlSerializer ser = XmlSerializerFactoryNoThrow.Create(typeof(T));

            using (StringWriter textWriter = new StringWriter())
            {
                ser.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public static T DeserializeFromFile<T>(string path) where T : class
        {
            string xmlInputData = File.ReadAllText(path);
            return Deserialize<T>(xmlInputData);
        }

        public void SerializeToFile<T>(T ObjectToSerialize, string path) where T : class
        {
            string xmlText = Serialize<T>(ObjectToSerialize);
            File.WriteAllText(path, xmlText);
        }

    }
}
