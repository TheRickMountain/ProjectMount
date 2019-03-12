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
        private Texture2D texture;

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

        public int X
        {
            get { return dest.X; }
            set { dest.X = value; }
        }

        public int Y
        {
            get { return dest.Y; }
            set { dest.Y = value; }
        }

        public int Widtth
        {
            get { return dest.Width; }
            set { dest.Width = value; }
        }

        public int Height
        {
            get { return dest.Height; }
            set { dest.Height = value; }
        }

        public Button(string name, Texture2D texture, bool active) : base(active)
        {
            Name = name;

            this.texture = texture;

            dest = new Rectangle(0, 0, texture.Width, texture.Height);

            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dest, color);
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
