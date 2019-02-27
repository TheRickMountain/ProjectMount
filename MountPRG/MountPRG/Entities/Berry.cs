using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Berry : Entity
    {
        public Berry()
        {
            Add(new Sprite(TextureBank.BerryTexture, false));
            Add(new Gatherable(ItemDatabase.GetItemById(ItemDatabase.BERRY)));
        }

    }
}
