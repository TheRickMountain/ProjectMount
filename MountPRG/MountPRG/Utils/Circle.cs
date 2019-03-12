using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class Circle
    {

        public float Radius
        {
            get;
        }

        public float X
        {
            get; set;
        }

        public float Y
        {
            get; set;
        }

        public Circle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public bool Intersects(Circle circle)
        {
            float distX = X - circle.X;
            float distY = Y - circle.Y;
            float dist = (float)Math.Sqrt(distX * distX + distY * distY);
            if (dist < Radius + circle.Radius)
                return true;

            return false;
        }

    }
}
