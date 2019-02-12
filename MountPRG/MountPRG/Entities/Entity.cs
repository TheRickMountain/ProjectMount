using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public abstract class Entity
    {
        public bool Active = true;
        public bool Visible = true;
        public Vector2 Position;

        public ComponentList Components { get; private set; }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        public Entity(Vector2 position)
        {    
            Position = position;
            Components = new ComponentList(this);
        }

        public Entity() 
            : this(Vector2.Zero)
        {

        }

        public virtual void Update(float dt)
        {
            Components.Update(dt);
        }


        public virtual void Render(SpriteBatch spriteBatch)
        {
            Components.Render(spriteBatch);
        }

        public void Add(Component component)
        {
            Components.Add(component);
            component.Initialize();
        }

        public void Remove(Component component)
        {
            Components.Remove(component);
        }

        public T Get<T>() where T : Component
        {
            return Components.Get<T>();
        }

    }
}
