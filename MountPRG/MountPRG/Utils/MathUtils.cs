using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class MathUtils
    {

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            float dX = x1 - x2;
            float dY = y1 - y2;
            return (float)Math.Sqrt((dX * dX) + (dY * dY));
        }

        public static float Lerp(float p1, float p2, float alpha)
        {
            return p1 + alpha * (p2 - p1);
        }

    }
}
