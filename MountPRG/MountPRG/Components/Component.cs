using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MountPRG.Entities;

namespace MountPRG.Components
{
    public abstract class Component
    {

        private Entity entity;

        public Entity Entity
        {
            get { return entity; }
        }

        public void Initialize(Entity entity)
        {
            this.entity = entity;
        }

    }
}
