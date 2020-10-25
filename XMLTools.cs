using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SharedClasses
{
    public class XMLSerializer
    {
        /// <summary>
        /// populate a class with xml data 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="input">xml data</param>
        /// <returns>Object Type</returns>
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

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
        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public T DeserializeFromFile<T>(string path) where T : class
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

    public class XMLStore
    {
        const string KEY = "key";
        const string VALUE = "value";
        public class KeyValue
        {
            public int key;
            public string value;
            public KeyValue(int _key, string _value)
            {
                key = _key;
                value = _value;
            }

            public object identityKey()
            {
                return key;
            }
        }

        public static KeyValue getObject(List<KeyValue> _list, int _id)
        {
            return _list.Where(x => x.key == _id).FirstOrDefault();
        }


        public static string keyValue(List<KeyValue> _list, ref short _listHead, int _Id)
        {
            while (_listHead < _list.Count)
            {
                if (_list[_listHead].key == _Id)
                    return _list[_listHead].value;
                ++_listHead;
            }
            return null;
        }



        public static string keyValue(List<KeyValue> _list, int _Id)
        {
            short _ = 0;
            return keyValue(_list, ref _, _Id);
        }

        public static XmlReader read(string _filePath)
        {
            return XmlReader.Create(_filePath);
        }

        public static string keyValue(string _filePath, int _key, short _languageId)
        {
            if (_languageId == 0)
                _languageId = 1;

            XmlReader reader = read(_filePath);
            string value = null;

            if (reader.ReadToDescendant(string.Format("L{0}", _languageId)))
            {
                int depth = reader.Depth;
                while (reader.Read() && reader.Depth > depth)
                {
                    if (reader.IsStartElement())
                    {
                        int key;
                        if (Int32.TryParse(reader[KEY], out key) && key == _key)
                        {
                            value = reader[VALUE];
                            break;
                        }
                    }
                }
            }
                
            reader.Close();
            return value;
        }

        public static List<KeyValue> xmlToList(string _filePath, string _groupId)
        {
            List<KeyValue> list = new List<KeyValue>();
            XmlReader reader = read(_filePath);

            if (reader.ReadToDescendant(_groupId))
            {
                int depth = reader.Depth;
                while (reader.Read() && reader.Depth > depth)
                {
                    if (reader.IsStartElement())
                    {
                        int key;
                        if (Int32.TryParse(reader[KEY], out key))
                        {
                            list.Add(new KeyValue(key, reader[VALUE]));
                        }
                    }
                }
            }

            reader.Close();

            return list;
        }

        public static List<KeyValue> xmlToList(string _filePath, short _languageId)
        {
            if (_languageId == 0)
                _languageId = 1;
            string groupId = string.Format("L{0}", _languageId);
            return xmlToList(_filePath, groupId);
        }
    }

    public class XMLTools
    {
        private static XmlReader read(string _filePath)
        {
            try
            {
                XmlReader reader = XmlReader.Create(_filePath);
                return reader;
            }
            catch
            {
                throw new Exception("cannot open file " + _filePath);
            }
        }
        public static Dictionary<string, string> loadDict(string _fileName, string descendant)
        {
            const string KEY = "key";
            const string VALUE = "value";
            XmlReader reader = read(_fileName);
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                if (reader.ReadToDescendant(descendant))
                {
                    int depth = reader.Depth;
                    while (reader.Read() && reader.Depth > depth)
                    {
                        if (reader.IsStartElement())
                        {
                            list.Add(reader[KEY], reader[VALUE]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error parsing file " + _fileName + ":" + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return list;
        }
        public static void saveDict(string _rootName, string _recordName, Dictionary<string, string> _dict, string _fileName)
        {
            string xml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?> \r\n <{0}> \r\r", _rootName);

            foreach (KeyValuePair<string, string> kv in _dict)
                xml += string.Format("<{0} key = \"{1}\" value = \"{2}\" > </{0}>\r\n", _recordName, kv.Key, kv.Value);

            xml += string.Format("</{0}> \r\r", _rootName);

               File.WriteAllText(_fileName, xml);
        }
    }
}
