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
        public Item Item
        {
            get; private set;
        }

        public int Count
        {
            get; set;
        }

        private Texture2D texture;

        public bool IsItemHolder
        {
            get; private set;
        }

        public Gatherable(Item item, int count, Texture2D texture) : base(false, true)
        {
            Item = item;
            Count = count;
            this.texture = texture;
            IsItemHolder = true;
        }

        public Gatherable(Item item, int count) : base(false, false)
        {
            Item = item;
            Count = count;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Parent.X, (int)Parent.Y, texture.Width, texture.Height), DayNightSystemGUI.CurrentColor);
        }

    }
}
