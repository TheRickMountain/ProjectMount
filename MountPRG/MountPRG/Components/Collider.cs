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
            get; private set;
        }

        public int Height
        {
            get; private set;
        }

        public int OffsetX
        {
            get; private set;
        }

        public int OffsetY
        {
            get; private set;
        }

        public Collider(int width, int height, int offsetX, int offsetY)
            : base(false, false)
        {
            Width = width;
            Height = height;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

    }
}
