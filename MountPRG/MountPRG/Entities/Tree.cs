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
            Tag = "Tree";
            Sprite sprite = new Sprite(ResourceBank.Sprites["tree"], false);
            sprite.Origin.X = 8;
            sprite.Origin.Y = 32;
            Add(sprite);
            Add(new Mineable(ItemDatabase.GetItemById(ItemDatabase.STICK), 5));
            Walkable = false;
        }

    }
}
