using System.Collections.Generic;

namespace SharedClasses
{
    public enum Compare
    {
       UNDETERMINED = -2, LESS, EQUAL, GREATER
    }

    public delegate Compare compare(object _x, object _y);
    public delegate bool equals(object _x, object _y);

    public class Sort
    {
        static public Compare compareInt(object _object1, object _object2)
        {
            return (int?)_object1 > (int?)_object2 ? Compare.GREATER : (int?)_object1 < (int?)_object2 ? Compare.LESS : Compare.EQUAL;
        }
        static public Compare compareStr(object _object1, object _object2)
        {
            int _ = ((string)_object1).CompareTo((string)_object2);

            return _ > 0 ? Compare.GREATER :_ < 0 ? Compare.LESS : Compare.EQUAL;
        }

        public static void swap<T>(List <T> _list, int first, int second)
        {
            T temporary = (T) _list [first];
            _list[first] = _list[second];
            _list[second] = temporary;
        }
        public static void selectionSort<T>(List <T> _list, Compare _order, compare _compare)
        {
            int max_min;
            for (int i = 0; i < _list.Count - 1; i++)
            {
                max_min = i;
                for (int index = i + 1; index < _list.Count; index++)
                {
                    if (_compare(_list[index], _list[max_min]) == _order)
                    {
                        max_min = index;
                    }
                }
                swap<T>(_list, i, max_min);
            }
        }
        public static void quickSort<T>(List<T> _list, int l, int r, Compare _order, compare _compare)
        {
            if (_list.Count == 0)
                return;

            int i, j;
            T x;

            i = l;
            j = r;

            x = _list[(l + r) / 2]; /* find pivot item */
            while (true)
            {
                while ((_compare(_list[i], x) == _order))
                    i++;
                while ((_compare(x, _list[j]) == _order))
                    j--;
                if (i <= j)
                {
                    swap(_list, i, j);
                    i++;
                    j--;
                }
                if (i > j)
                    break;
            }
            if (l < j)
                quickSort(_list, l, j, _order, _compare);
            if (i < r)
                quickSort(_list, i, r, _order, _compare);
        }

        public static void quickSort<T>(List<T> _list, Compare _order, compare _compare)
        {
            quickSort(_list, 0, _list.Count - 1, _order, _compare);
        }
    }
}