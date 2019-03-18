using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Button : IGUI
    {
        private Texture2D background;

        public Texture2D Icon
        {
            get; set;
        }

        private SpriteFont font;

        public string Text
        {
            get; set;
        }

        private Rectangle dest;

        private Color color;

        private bool selected;

        private Action<Button> cbButtonDown;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                color = selected ? Color.Orange : Color.White;
            }
        }
     
        public string Name
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

        public Button(Texture2D background, Texture2D icon, bool active) : base(active)
        {
            this.background = background;
            this.Icon = icon;

            dest = new Rectangle(0, 0, background.Width, background.Height);

            color = Color.White;
        }

        public Button(Texture2D background, string text, bool active) : base(active)
        {
            this.background = background;
            Text = text;

            Vector2 textSize = ResourceBank.Fonts["mountFont"].MeasureString(text);

            dest = new Rectangle(0, 0, (int)textSize.X, (int)textSize.Y);

            font = ResourceBank.Fonts["mountFont"];

            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, dest, color);
            if (Icon != null)
                spriteBatch.Draw(Icon, dest, color);
            
            if(Text != null)
                spriteBatch.DrawString(font, Text, new Vector2(dest.X, dest.Y), Color.White);
        }

        public bool Intersects(int x, int y)
        {
            if (x >= dest.X && x <= dest.Right &&
                y >= dest.Y && y <= dest.Bottom)
                return true;

            return false;
        }

        public void OnButtonDownCallback(Action<Button> cb)
        {
            cbButtonDown += cb;
        }

        public void ButtonDown()
        {
            cbButtonDown(this);
        }
    }
}
