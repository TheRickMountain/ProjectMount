using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class SpriteUI : UI
    {

        public Texture2D Texture
        {
            get; set;
        }

        private Rectangle dest;

        public Color Color = Color.White;

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

        public SpriteUI(Texture2D texture) : this(texture, texture.Width, texture.Height)
        {
        }

        public SpriteUI(Texture2D texture, int width, int height)
        {
            Texture = texture;
            dest = new Rectangle(0, 0, width, height);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
                spriteBatch.Draw(Texture, dest, Color);
        }

        public override bool Intersects(int x, int y)
        {
            return dest.Contains(new Point(x, y));
        }

    }
}
