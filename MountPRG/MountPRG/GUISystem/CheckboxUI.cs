using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class CheckboxUI : UI
    {
        private Texture2D background;
        private Texture2D mark;

        private Rectangle dest;

        private Action<CheckboxUI> cbCheckboxDown;

        public bool Marked
        {
            get; set;
        }

        public Color Color
        {
            get; set;
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

        public CheckboxUI()
        {
            background = ResourceBank.Sprites["checkbox"];
            mark = ResourceBank.Sprites["mark"];

            dest = new Rectangle(0, 0, GUIManager.CHECKBOX_SIZE, GUIManager.CHECKBOX_SIZE);

            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                Marked = Intersects(InputManager.GetX(), InputManager.GetY());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, dest, Color);
            if (Marked)
                spriteBatch.Draw(mark, dest, Color);
        }

        public void OnCheckboxDownCallback(Action<CheckboxUI> cb)
        {
            cbCheckboxDown += cb;
        }

        public bool GetCheckboxDown()
        {
            if (Intersects(InputManager.GetX(), InputManager.GetY()))
            {
                if (!Marked)
                {
                    cbCheckboxDown(this);
                    return Marked = true;
                }
                else
                {
                    return Marked = false;
                }
            }

            return false;
        }

        public override bool Intersects(int x, int y)
        {
            return dest.Contains(new Point(x, y));
        }

    }
}
