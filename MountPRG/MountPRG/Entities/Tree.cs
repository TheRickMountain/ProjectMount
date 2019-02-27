using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Tree : Entity
    {

        public Tree()
        {
            Sprite sprite = new Sprite(TextureBank.TreeTexture, false);
            sprite.Origin.X = 8;
            sprite.Origin.Y = 32;
            Add(sprite);
            Walkable = false;
        }

    }
}
