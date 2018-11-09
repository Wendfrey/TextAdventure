using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    static class CustomMath
    {
        public static Random random = new Random();

        public static int Abs(int a)
        {
            return (a < 0)? -a : a;
        }

        public static double RandomUnit()
        {
            return random.Next() / (1d * int.MaxValue);
        }

        /**
         *Devuelve un integer entre max (incluido) y min (incluido)
         * 
         */
        public static int RandomIntNumber(int max=int.MaxValue, int min=0)
        {
            if(min > max)
            {
                return -1;
            }

            int temp = max - min;
            return min + ((int)(RandomUnit() * (1 + temp)));
        }

        public static bool LowerThan(int a, int b)
        {
            return (a < b);
        }

        public static int ExpNeeded(int lvl)
        {
            return lvl*lvl*lvl;
        }
    }
}
