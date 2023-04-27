using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
    public class MergeSortAlgorythm
    {
        public static void MergeSort<T>(List<T> list) where T : IComparable<T>
        {
            MergeSort(list, 0, list.Count - 1);
        }

        private static void MergeSort<T>(List<T> list, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;

            int mid = (left + right) / 2;
            MergeSort(list, left, mid);
            MergeSort(list, mid + 1, right);
            Merge(list, left, mid, right);
        }

        private static void Merge<T>(List<T> list, int left, int mid, int right) where T : IComparable<T>
        {
            List<T> temp = new List<T>(right - left + 1);
            int i = left;
            int j = mid + 1;

            while (i <= mid && j <= right)
            {
                if (list[i].CompareTo(list[j]) <= 0)
                    temp.Add(list[i++]);
                else
                    temp.Add(list[j++]);
            }

            while (i <= mid)
                temp.Add(list[i++]);

            while (j <= right)
                temp.Add(list[j++]);

            for (int l = 0; l < temp.Count; l++)
                list[left + l] = temp[l];
        }
    }
}
