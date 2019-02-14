using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class ButtonGUI : IGUI
    {

        private Sprite sprite;

        public int X
        {
            get { return sprite.Destination.X; }
            set { sprite.Destination.X = value; }
        }

        public int Y
        {
            get { return sprite.Destination.Y; }
            set { sprite.Destination.Y = value; }
        }

        public int Width
        {
            get { return sprite.Destination.Width; }
            set { sprite.Destination.Width = value; }
        }

        public int Height
        {
            get { return sprite.Destination.Height; }
            set { sprite.Destination.Height = value; }
        }

        public ButtonGUI(Texture2D texture)
        {
            sprite = new Sprite(texture, false);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public bool Pressed(MouseInput mouseInput)
        {
            if (InputManager.MousePressed(mouseInput))
                if (sprite.Intersects(InputManager.GetX(), InputManager.GetY()))
                    return true;

            return false;
        }

        public bool Down(MouseInput mouseInput)
        {
            if (InputManager.MouseDown(mouseInput))
                if (sprite.Intersects(InputManager.GetX(), InputManager.GetY()))
                    return true;

            return false;
        }

        public bool Released(MouseInput mouseInput)
        {
            if (InputManager.MouseReleased(mouseInput))
                if (sprite.Intersects(InputManager.GetX(), InputManager.GetY()))
                    return true;

            return false;
        }
    }
}
