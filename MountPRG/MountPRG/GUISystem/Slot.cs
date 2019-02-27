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

        private Texture2D background;
        private Rectangle dest;

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

        public Slot(Texture2D background, int width, int height, bool active) : base(active)
        {
            this.background = background;

            dest = new Rectangle(0, 0, width, height);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, dest, Color.White);
        }

        public bool Intersects(int x, int y)
        {
            if (x >= dest.X && x <= dest.Right &&
                y >= dest.Y && y <= dest.Bottom)
                return true;

            return false;
        }
        
    }
}
