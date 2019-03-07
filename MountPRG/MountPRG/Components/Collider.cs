using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Collider : Component
    {

        public int Width
        {
            get;
        }

        public int Height
        {
            get;
        }

        public int HalfWidth
        {
            get { return Width / 2; }
        }

        public int HalfHeight
        {
            get { return Height / 2; }
        }

        public Collider(int width, int height) : base(false, false)
        {
            Width = width;
            Height = height;
        }

    }
}
