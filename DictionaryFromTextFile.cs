using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{

public static class DictionaryLoader
    {
        public static Dictionary<string, string> LoadDictionaryFromFile(string filePath)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Process each line
                foreach (string line in lines)
                {
                    // Split the line by '=' to get key and value
                    string[] keyValue = line.Split(new char[] { '=' }, 2);

                    // Ensure we have a valid key-value pair
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();

                        // Add the key-value pair to the dictionary
                        dictionary[key] = value;
                    }
                    else
                    {
                        // Handle invalid lines or missing '='
                        Console.WriteLine($"Invalid line: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as file not found or access issues
                Console.WriteLine($"Error while reading the file: {ex.Message}");
            }

            return dictionary;
        }
    }

}

