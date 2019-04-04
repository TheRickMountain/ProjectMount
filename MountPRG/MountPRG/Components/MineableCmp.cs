using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Mineable : Component
    {

        public Item Item
        {
            get; private set;
        }

        public int Count
        {
            get; private set;
        }

        public Mineable(Item item, int count)
            : base(false, false)
        {
            Item = item;
            Count = count;
        }

        public override Component Clone()
        {
            throw null;
        }
    }
}
