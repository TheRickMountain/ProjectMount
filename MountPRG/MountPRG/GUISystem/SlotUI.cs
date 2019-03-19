using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class SlotUI : UI
    {
        public Item Item
        {
            get; private set;
        }

        public int Count
        {
            get; private set;
        }

        private SpriteFont font;

        private Texture2D background;
        private Rectangle dest;

        private Sprite itemSprite;

        public bool HasItem
        {
            get; private set;
        }

        public float X
        {
            get { return dest.X; }
            set { dest.X = (int)value; }
        }

        public float Y
        {
            get { return dest.Y; }
            set { dest.Y = (int)value; }
        }

        public float Width
        {
            get { return dest.Width; }
            set { dest.Width = (int)value; }
        }

        public float Height
        {
            get { return dest.Height; }
            set { dest.Height = (int)value; }
        }

        public SlotUI(Texture2D background, int width, int height)
        {
            Active = false;

            this.background = background;

            font = ResourceBank.Fonts["mountFont"];

            dest = new Rectangle(0, 0, width, height);

            HasItem = false;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, dest, Color.White);
            if(HasItem)
                spriteBatch.Draw(itemSprite.Texture, new Rectangle(dest.X + 4, dest.Y + 4,
                    dest.Width - 8, dest.Height - 8), itemSprite.Source, Color.White);
            if (Count > 1)
                spriteBatch.DrawString(font, "" + Count, new Vector2(dest.X + 2, dest.Y), Color.White);
        }

        public override bool Intersects(int x, int y)
        {
            return dest.Contains(new Point(x, y));
        }

        public void AddItem(Item item, int count)
        {
            Item = item;
            Count = count;

            itemSprite = item.Sprite;
            HasItem = true;
        }

        public void Clear()
        {
            Item = null;
            Count = 0;

            itemSprite = null;
            HasItem = false;
        }
        
    }
}
