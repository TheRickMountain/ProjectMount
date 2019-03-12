using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Stick : Entity
    {
        public Stick()
        {
            Add(new Sprite(ResourceBank.Sprites["stick"], false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.STICK), 1));
        }

    }
}
