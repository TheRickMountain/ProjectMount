﻿using System;
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

        protected Game GameRef;

        public Texture2D Texture;
        public Vector2 Position;
        public bool IsGatherable;
        public bool IsBush;
        public bool IsEatable;

        protected readonly List<Component> components = new List<Component>();

        public Entity(Game game)
        {
            GameRef = game;
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
