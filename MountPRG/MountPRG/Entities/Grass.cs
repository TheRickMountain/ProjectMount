using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Grass : Entity
    {
        public Grass()
        {
            Add(new Sprite(ResourceBank.Sprites["grass"], false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.HAY), 1));
        }

    }
}
