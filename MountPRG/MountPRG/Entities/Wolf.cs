using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Wolf : Entity
    {

        public Wolf(float x, float y) : base(x, y)
        {
            Sprite sprite = new Sprite(ResourceBank.WolfTexture, true);
            sprite.Origin.X = 8;
            sprite.Origin.Y = 12;
            Add(sprite);

            Add(new AIController());
        }

    }
}
