using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class TimeSystemGUI : IGUI
    {

        private float currentTime;

        private Texture2D circle;
        private Texture2D arrow;

        private Rectangle circleDestination;
        private Rectangle circleSource;
        private Vector2 circleOrigin;

        private Rectangle arrowDestination;

        public TimeSystemGUI(Game game)
        {
            circle = game.Content.Load<Texture2D>(@"day_night");
            arrow = game.Content.Load<Texture2D>(@"arrow");

            circleDestination = new Rectangle(40, 40, 80, 80);
            circleSource = new Rectangle(0, 0, 32, 32);
            circleOrigin = new Vector2(16, 16);

            arrowDestination = new Rectangle(0, 0, 80, 80);
        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)(gameTime.ElapsedGameTime.TotalSeconds * 0.1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circle, circleDestination, circleSource, Color.White, currentTime, circleOrigin, SpriteEffects.None, 1);
            spriteBatch.Draw(arrow, arrowDestination, Color.White);
        }

    }
}
