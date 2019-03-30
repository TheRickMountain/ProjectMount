using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class ButtonUI : UI
    {
        private Texture2D background;

        private Rectangle dest;

        private Color color;

        private bool selected;

        private Action<ButtonUI> cbButtonDown;

        public Texture2D Icon
        {
            get; set;
        }

        public TextUI Text
        {
            get; private set;
        }

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

        public ButtonUI(Texture2D background, Texture2D icon)
        {
            Active = false;

            this.background = background;
            Icon = icon;

            dest = new Rectangle(0, 0, background.Width, background.Height);

            color = Color.White;
        }

        public ButtonUI(Texture2D background, TextUI text)
        {
            Active = false;

            this.background = background;

            Text = text;

            dest = new Rectangle(0, 0, Text.Width, Text.Height);

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

            if (Text != null)
            {
                Text.X = dest.X;
                Text.Y = dest.Y;
                Text.Draw(spriteBatch);
            }
        }

        public override bool Intersects(int x, int y)
        {
            return dest.Contains(new Point(x, y));
        }

        public void OnButtonDownCallback(Action<ButtonUI> cb)
        {
            cbButtonDown += cb;
        }

        public bool GetButtonDown()
        {
            if (Intersects(InputManager.GetX(), InputManager.GetY()))
            {
                cbButtonDown(this);
                return true;
            }

            return false;
        }
    }
}
