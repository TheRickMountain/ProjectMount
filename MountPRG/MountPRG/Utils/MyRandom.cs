using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class MyRandom
    {

        private static Random random = new Random();

        public static int Range(int min, int max)
        {
            return random.Next(min, max + 1);
        }

    }
}
