using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class ComponentList 
    {
        public Entity Entity { get; internal set; }

        private List<Component> components;
        private List<Component> toAdd;
        private List<Component> toRemove;

        private HashSet<Component> current;

        internal ComponentList(Entity entity)
        {
            Entity = entity;

            components = new List<Component>();
            toAdd = new List<Component>();
            toRemove = new List<Component>();

            current = new HashSet<Component>();
        }

        public void Add(Component component)
        {
            if (!current.Contains(component))
            {
                current.Add(component);
                components.Add(component);
                component.Added(Entity);
            }
        }

        public void Remove(Component component)
        {
            if(current.Contains(component))
            {
                current.Remove(component);
                components.Remove(component);
                component.Removed(Entity);
            }
        }

        public int Count
        {
            get
            {
                return components.Count;
            }
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var component in components)
                if (component.Active)
                    component.Update(gameTime);              
        }

        internal void Render(SpriteBatch spriteBatch)
        {
            foreach (var component in components)
                if (component.Visible)
                    component.Render(spriteBatch);
        }

        public T Get<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    return component as T;
            return null;
        }

    }
}
