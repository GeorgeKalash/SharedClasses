using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public static class KeyGenerator
    {
        public static double generate(short _length)
        {
            double n = 0;
            for (int i = _length - 1; i >= 0; i--)
            {
                Random random = new Random();
                int oneDigitNumber = random.Next(0, 10);
                n += oneDigitNumber * Math.Pow(10, i);
            }

            return n;
        }
    }

}
