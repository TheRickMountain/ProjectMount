using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Flint : Entity
    {
        public Flint()
        {
            Add(new Sprite(ResourceBank.Sprites["flint"], false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.FLINT), 1));
        }

    }
}
