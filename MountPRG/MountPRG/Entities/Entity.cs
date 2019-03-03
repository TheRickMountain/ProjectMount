using System;
using System.Collections;
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
        public bool Walkable = true;
        public string Tag = "";

        internal int depth = 0;
        internal int actualDepth = 0;

        public ComponentList Components { get; private set; }

        public float X
        {
            get; set;
        }
     
        public float Y
        {
            get; set;
        }

        public float VelX
        {
            get; set;
        }

        public float VelY
        {
            get; set;
        }

        // Установление очередности прорисовки объектов
        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    // Сообщить EntityList что необходимо произвести сортировку
                    GamePlayState.Entities.MarkUnsorted();
                }
            }
        }

        public Entity(float x, float y)
        {
            X = x;
            Y = y;
            Components = new ComponentList(this);
        }

        public Entity() 
            : this(0, 0)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            Components.Update(gameTime);
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

        public bool Has<T>() where T : Component
        {
            if (Components.Get<T>() != null)
                return true;

            return false;
        }
    }
}
