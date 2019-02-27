using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Gatherable : Component
    {
        public Item Item
        {
            get; private set;
        }

        public Gatherable(Item item) : base(false, false)
        {
            Item = item;
        }

    }
}
