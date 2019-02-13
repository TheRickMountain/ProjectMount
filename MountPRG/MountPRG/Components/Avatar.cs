using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Avatar : Component
    {
        public Texture2D Texture
        {
            get;
            private set;
        }

        public Avatar(Texture2D texture)
            : base(false, false)
        {
            Texture = texture;
        }
    }
}
