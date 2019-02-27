using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Wood : Entity
    {

        public Wood()
        {
            Add(new Sprite(TextureBank.WoodTexture, false));
        }

    }
}
