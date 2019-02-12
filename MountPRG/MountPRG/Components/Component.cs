﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public abstract class Component
    {

        public Entity Entity { get; private set; }
        public bool Active;
        public bool Visible;

        public Component(bool active, bool visible)
        {
            Active = active;
            Visible = visible;
        }

        public virtual void Added(Entity entity)
        {
            Entity = entity;
        }

        public virtual void Removed(Entity entity)
        { 
            Entity = null;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Render(SpriteBatch spriteBatch)
        {

        }

    }
}
