using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MountPRG.Components;

namespace MountPRG.Entities
{
    public abstract class Entity : IComparable<Entity>
    {

        public int Id
        {
            get;
            protected set;
        }

        public static int STICK = 0;

        protected Game GameRef;

        public Vector2 Position;

        protected readonly List<Component> components = new List<Component>();

        public Entity(Game game, int id)
        {
            GameRef = game;
            Id = id;
        }

        public abstract void Update(GameTime gameTime);


        public abstract void Draw(SpriteBatch spriteBatch);

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
                if (component is T)
                    return component as T;

            return null;
        }

        public void AddComponent(Component component)
        {
            component.Initialize(this);
            components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        public bool HasComponent<T>() where T : Component
        {
            foreach (Component component in components)
                if (component is T)
                    return true;

            return false;
        }

        public int CompareTo(Entity other)
        {
            throw new NotImplementedException();
        }
    }
}
