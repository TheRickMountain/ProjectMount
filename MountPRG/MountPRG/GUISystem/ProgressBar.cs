using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.GUISystem
{
    public class ProgressBar : IGUI
    {
        private Color color;
        private Texture2D texture;
        private Rectangle backgroundDest;
        private Rectangle midlineDest;

        public int X
        {
            get { return backgroundDest.X; }
            set
            {
                backgroundDest.X = value;
                midlineDest.X = value + 2;
            }
        }

        public int Y
        {
            get { return backgroundDest.Y; }
            set
            {
                backgroundDest.Y = value;
                midlineDest.Y = value + 2;
            }
        }

        public int Width
        {
            get { return backgroundDest.Width; }
            set
            {
                backgroundDest.Width = value;
                midlineDest.Width = value - 4;
            }
        }

        public int Height
        {
            get { return backgroundDest.Height; }
            set
            {
                backgroundDest.Height = value;
                midlineDest.Height = value - 4;
            }
        }

        public ProgressBar(Game game, Color color)
        {
            this.color = color;
            texture = game.Content.Load<Texture2D>(@"progressBar");
            backgroundDest = new Rectangle();
            midlineDest = new Rectangle();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, backgroundDest, Color.Black);
            spriteBatch.Draw(texture, midlineDest, color);
        }

    }
}
