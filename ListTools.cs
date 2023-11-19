using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public static class ListTools
    {
        public static List<int> RemoveDuplicates(List<int> inputList)
        {
            // Use a HashSet to keep track of unique values
            HashSet<int> uniqueValues = new HashSet<int>();

            // Create a new list to store the result without duplicates
            List<int> resultList = new List<int>();

            foreach (int value in inputList)
            {
                // If the value is not in the HashSet, add it to both the HashSet and the result list
                if (uniqueValues.Add(value))
                {
                    resultList.Add(value);
                }
            }

            return resultList;
        }
    }
}
