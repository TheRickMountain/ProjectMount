using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class StorageGUI : IGUI
    {

        private Sprite sprite;
        private ButtonGUI closeButton;

        public int X
        {
            get { return sprite.Destination.X; }
            set
            {
                sprite.Destination.X = value;

                closeButton.X = sprite.Destination.Right - closeButton.Width - 1;
                closeButton.Y = sprite.Destination.Y + 1;
            }
        }

        public int Y
        {
            get { return sprite.Destination.Y; }
            set
            {
                sprite.Destination.Y = value;

                closeButton.X = sprite.Destination.Right - closeButton.Width - 1;
                closeButton.Y = sprite.Destination.Y + 1;
            }
        }

        public int Width
        {
            get { return sprite.Destination.Width; }
            set
            {
                sprite.Destination.Width = value;

                closeButton.X = sprite.Destination.Right - closeButton.Width - 1;
                closeButton.Y = sprite.Destination.Y + 1;
            }
        }

        public int Height
        {
            get { return sprite.Destination.Height; }
            set
            {
                sprite.Destination.Height = value;

                closeButton.X = sprite.Destination.Right - closeButton.Width - 1;
                closeButton.Y = sprite.Destination.Y + 1;
            }
        }

        public StorageGUI()
        {
            sprite = new Sprite(GamePlayState.ProgressBarTexture, false);
            sprite.Color = Color.Gray;

            closeButton = new ButtonGUI(GamePlayState.CrossTexture);
            closeButton.Width *= 2;
            closeButton.Height *= 2;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            closeButton.Draw(spriteBatch);
        }

    }
}
