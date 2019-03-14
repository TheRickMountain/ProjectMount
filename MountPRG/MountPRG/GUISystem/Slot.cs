using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Slot : IGUI
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

        private Texture2D backgroundTexture;
        private Rectangle backgroundDest;

        private Sprite itemSprite;

        public bool HasItem
        {
            get; private set;
        }

        public float X
        {
            get { return backgroundDest.X; }
            set { backgroundDest.X = (int)value; }
        }

        public float Y
        {
            get { return backgroundDest.Y; }
            set { backgroundDest.Y = (int)value; }
        }

        public float Width
        {
            get { return backgroundDest.Width; }
            set { backgroundDest.Width = (int)value; }
        }

        public float Height
        {
            get { return backgroundDest.Height; }
            set { backgroundDest.Height = (int)value; }
        }

        public Slot(Texture2D background, int width, int height, bool active) : base(active)
        {
            backgroundTexture = background;

            font = ResourceBank.Fonts["mountFont"];

            backgroundDest = new Rectangle(0, 0, width, height);

            HasItem = false;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundDest, Color.White);
            if(HasItem)
                spriteBatch.Draw(itemSprite.Texture, new Rectangle(backgroundDest.X + 4, backgroundDest.Y + 4,
                    backgroundDest.Width - 8, backgroundDest.Height - 8), itemSprite.Source, Color.White);
            if (Count > 1)
                spriteBatch.DrawString(font, "" + Count, new Vector2(backgroundDest.X + 2, backgroundDest.Y), Color.White);
        }

        public bool Intersects(int x, int y)
        {
            if (x >= backgroundDest.X && x <= backgroundDest.Right &&
                y >= backgroundDest.Y && y <= backgroundDest.Bottom)
                return true;

            return false;
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
