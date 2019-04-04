using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class GatherableCmp : Component
    {

        private Action cbCountEqualsZero;

        public Item Item
        {
            get; private set;
        }

        private int count;

        public int Count
        {
            get { return count; }
            set {
                count = value;
                if (count == 0)
                    cbCountEqualsZero();
            }
        }

        public bool ItemHolder
        {
            get; private set;
        }

        public GatherableCmp(Item item, int count, bool itemHolder = false)
            : base(false, false)
        {
            Item = item;
            Count = count;
            ItemHolder = itemHolder;
        } 

        public void OnCountEqualsZeroCallback(Action cb)
        {
            cbCountEqualsZero += cb;
        }

        public override Component Clone()
        {
            throw null;
        }
    }
}
