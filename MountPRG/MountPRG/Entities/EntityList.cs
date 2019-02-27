using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class EntityList
    {
        private List<Entity> entities;
        private List<Entity> toAdd;
        private List<Entity> toRemove;

        private HashSet<Entity> current;
        private HashSet<Entity> adding;
        private HashSet<Entity> removing;

        private bool unsorted;

        internal EntityList()
        {
            entities = new List<Entity>();
            toAdd = new List<Entity>();
            toRemove = new List<Entity>();

            current = new HashSet<Entity>();
            adding = new HashSet<Entity>();
            removing = new HashSet<Entity>();
        }

        internal void MarkUnsorted()
        {
            unsorted = true;
        }

        public void UpdateList()
        {
            if (toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var entity = toAdd[i];
                    if (!current.Contains(entity))
                    {
                        current.Add(entity);
                        entities.Add(entity);
                    }
                }

                toAdd.Clear();
                adding.Clear();

                unsorted = true;
            }

            if(toRemove.Count > 0)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    var entity = toRemove[i];
                    if (entities.Contains(entity))
                    {
                        current.Remove(entity);
                        entities.Remove(entity);
                    }
                }

                toRemove.Clear();
                removing.Clear();
            }

            if(unsorted)
            {
                unsorted = false;
                entities.Sort(CompareDepth);
            }
        }

        public void Add(Entity entity)
        {
            if(!current.Contains(entity))
            {
                adding.Add(entity);
                toAdd.Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (!removing.Contains(entity) && current.Contains(entity))
            {
                removing.Add(entity);
                toRemove.Add(entity);
            }
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
                if (entity.Active)
                    entity.Update(gameTime);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                if (entity.Visible)
                    entity.Render(spriteBatch);
        }

        public static Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(a.depth - b.depth); };
    }
}
