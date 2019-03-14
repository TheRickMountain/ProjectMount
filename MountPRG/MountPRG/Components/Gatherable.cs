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

        private Action cbCountEqualsZero;

        public Entity Entity
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

        public bool EntityHolder
        {
            get; private set;
        }

        public Gatherable(Entity entity, int count, bool entityHolder = false) : base(false, false)
        {
            Entity = entity;
            this.count = count;
            EntityHolder = entityHolder;
        } 

        public void OnCountEqualsZeroCallback(Action cb)
        {
            cbCountEqualsZero += cb;
        }

    }
}
