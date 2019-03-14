using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Gatherable : Component
    {
        public Item Item
        {
            get; private set;
        }

        public int Count
        {
            get; set;
        }

        public Gatherable(Item item, int count) : base(false, false)
        {
            Item = item;
            Count = count;
        } 

    }
}
