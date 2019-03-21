using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class SliderUI : UI
    {

        private Color back;
        private Color front;

        private Rectangle dest;

        private Texture2D texture;

        private float progress;

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

        public int Width
        {
            get { return dest.Width; }
            set { dest.Width = value; }
        }

        public int Height
        {
            get { return dest.Height; }
            set { dest.Height = value; }
        }

        public SliderUI(int width, int height, Color back, Color front)
        {
            this.back = back;
            this.front = front;

            dest = new Rectangle(0, 0, width, height);

            texture = ResourceBank.Sprites["slider"];
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dest, back);
            spriteBatch.Draw(texture, new Rectangle(dest.X, dest.Y, (int)progress, dest.Height), front);
        }

        public void SetValue(float currValue, float maxValue)
        {
            progress = currValue / maxValue * dest.Width;
        }

        public void Reset()
        {
            progress = 0;
        }

        public override bool Intersects(int x, int y)
        {
            return false;
        }
    }
}
