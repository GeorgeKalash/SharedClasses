using System;
using System.Collections.Generic;
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
    }
}
