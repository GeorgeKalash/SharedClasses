using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SharedClasses
{
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
