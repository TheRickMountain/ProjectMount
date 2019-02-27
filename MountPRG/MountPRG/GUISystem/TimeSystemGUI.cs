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

        public static Color DayColor
        {
            get; private set;
        }

        public static Color NightColor
        {
            get; private set;
        }

        public static Color CurrentColor
        {
            get; private set;
        }

        public TimeSystemGUI(Game game)
        {
            circle = game.Content.Load<Texture2D>(@"day_night");
            arrow = game.Content.Load<Texture2D>(@"arrow");

            circleDestination = new Rectangle(40, 40, 80, 80);
            circleSource = new Rectangle(0, 0, 32, 32);
            circleOrigin = new Vector2(16, 16);

            arrowDestination = new Rectangle(0, 0, 80, 80);

            DayColor = new Color(255, 255, 255, 255);
            NightColor = new Color(63, 137, 255, 255);
            CurrentColor = DayColor;
        }

        public void Update(GameTime gameTime)
        {
            currentTime -= (float)(gameTime.ElapsedGameTime.TotalSeconds * 0.1);
            int timeOfDay = -MathUtils.ToDegrees(currentTime);
            if(timeOfDay >= 80 && timeOfDay <= 110)
            {
                float amount = Math.Min(1.0f, (timeOfDay - 80.0f) / 20.0f);
                CurrentColor = Color.Lerp(DayColor, NightColor, amount);
            }
            else if(timeOfDay >= 260 && timeOfDay <= 280)
            {
                float amount = Math.Min(1.0f, (timeOfDay - 260.0f) / 20.0f);
                CurrentColor = Color.Lerp(NightColor, DayColor, amount);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circle, circleDestination, circleSource, Color.White, currentTime, circleOrigin, SpriteEffects.None, 1);
            spriteBatch.Draw(arrow, arrowDestination, Color.White);
        }

    }
}
