using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Slider : IGUI
    {
        private Texture2D icon;
        private Texture2D background;

        private Rectangle dest;
        private Rectangle iconDest;

        private Rectangle leftSrc;
        private Rectangle centerSrc;
        private Rectangle rightSrc;

        public Slider(Texture2D icon) : base(true)
        {
            this.icon = icon;
            background = ResourceBank.Sprites["slider"];

            dest = new Rectangle(0, 90, 100, 16);
            iconDest = new Rectangle(dest.X, dest.Y, 16, 16);

            leftSrc = new Rectangle(0, 0, background.Width / 2, background.Height);
            centerSrc = new Rectangle(background.Width / 2 - (background.Width / 2) / 2, 0, background.Width / 2, background.Height);
            rightSrc = new Rectangle(background.Width / 2, 0, background.Width / 2, background.Height);
        }
        
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, iconDest, Color.White);
            spriteBatch.Draw(background, new Rectangle(iconDest.Width + dest.X, dest.Y, 8, dest.Height), leftSrc, Color.White);
            spriteBatch.Draw(background, new Rectangle(iconDest.Width + dest.X + 8, dest.Y, dest.Width, dest.Height), centerSrc, Color.White);
            spriteBatch.Draw(background, new Rectangle(iconDest.Width + dest.X + 8 + dest.Width, dest.Y, 8, dest.Height), rightSrc, Color.White);
        }

    }
}
