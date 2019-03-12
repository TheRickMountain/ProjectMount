using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Bush : Entity
    {
        public Bush()
        {
            Add(new Sprite(ResourceBank.Sprites["bush"], false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.BERRY), 3, ResourceBank.Sprites["raspberry"]));
            Walkable = false;
        }

    }
}
