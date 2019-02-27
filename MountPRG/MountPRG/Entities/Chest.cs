using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Chest : Entity
    {

        public Chest()
            : base()
        {
            Add(new Sprite(TextureBank.ChestTexture, false));
            Add(new Storage(3, 3));
            Walkable = false;
        }

    }
}
