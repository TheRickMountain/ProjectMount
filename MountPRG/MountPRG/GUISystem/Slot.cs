using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.GUISystem
{
    public class Slot : IGUI
    {

        private Texture2D texture;
        private Rectangle dest;

        public int PositionX
        {
            get { return dest.X; }
            set { dest.X = value; }
        }

        public int PositionY
        {
            get { return dest.Y; }
            set { dest.Y = value; }
        }

        public int Width
        {
            get { return dest.Width; }
        }

        public int Height
        {
            get { return dest.Height; }
        }

        public Slot(Texture2D texture, int width, int height)
        {
            this.texture = texture;
            dest = new Rectangle(0, 0, width, height);
        }
        
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dest, Color.White);
        }

    }
}
