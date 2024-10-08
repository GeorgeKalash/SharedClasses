﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
    public class MergeSort
    {
        public static void sort<T>(List<T> list) where T : IComparable<T>
        {
            mergeSort(list, 0, list.Count - 1);
        }

        private static void mergeSort<T>(List<T> list, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;

            int mid = (left + right) / 2;
            mergeSort(list, left, mid);
            mergeSort(list, mid + 1, right);
            merge(list, left, mid, right);
        }

        private static void merge<T>(List<T> list, int left, int mid, int right) where T : IComparable<T>
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


        // Merge Sort with delegate

        public static void sort<T>(List<T> list, Comparison<T> comparison)
        {
            mergeSort(list, 0, list.Count - 1, comparison);
        }
        private static void mergeSort<T>(List<T> list, int left, int right,Comparison<T> comparison) 
        {
            if (left >= right)
                return;

            int mid = (left + right) / 2;
            mergeSort(list, left, mid, comparison);
            mergeSort(list, mid + 1, right, comparison);
            merge(list, left, mid, right, comparison);
        }

        private static void merge<T>(List<T> list, int left, int mid, int right, Comparison<T> comparison) 
        {
            List<T> temp = new List<T>(right - left + 1);
            int i = left;
            int j = mid + 1;

            while (i <= mid && j <= right)
            {
                if (comparison(list[i],list[j]) <= 0)
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
