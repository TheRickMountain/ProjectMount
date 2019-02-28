using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Stone : Entity
    {
        public Stone()
        {
            Add(new Sprite(ResourceBank.StoneTexture, false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.STONE)));
        }

    }
}
