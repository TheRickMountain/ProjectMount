using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class TextUI : UI
    {

        private SpriteFont font;

        private Vector2 position;
        private Vector2 size;

        private string text;

        public string Text
        {
            get { return text; }
            set {
                text = value;
                size = font.MeasureString(text);
            }
        }

        public Color Color = Color.White;

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public int Width
        {
            get { return (int)size.X; }
        }

        public int Height
        {
            get { return (int)size.Y; }
        }

        public TextUI(SpriteFont font, string text)
        {
            Active = false;

            this.font = font;

            Text = text;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, Color);
        }

        public override bool Intersects(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }

    }
}
