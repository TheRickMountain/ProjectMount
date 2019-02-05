using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG.Components
{
    public class Collider : Component
    {

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }

        public int OffsetX
        {
            get; set;
        }

        public int OffsetY
        {
            get; set;
        }

        public Collider(int width, int height, int offsetX, int offsetY)
        {
            Width = width;
            Height = height;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

    }
}
