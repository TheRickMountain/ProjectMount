﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MountPRG
{
    public class TimeSystemUI : UI
    {
        private Texture2D circle;
        private Texture2D arrow;

        private Rectangle circleDestination;
        private Rectangle circleSource;
        private Vector2 circleOrigin;

        private Rectangle arrowDestination;

        private float rotation;

        public Color DayColor
        {
            get; private set;
        }

        public Color NightColor
        {
            get; private set;
        }

        public Color CurrentColor
        {
            get; private set;
        }

        public TimeSystemUI()
        {
            circle = ResourceBank.Sprites["day_night"];
            arrow = ResourceBank.Sprites["arrow"];

            circleDestination = new Rectangle(40, 40, 80, 80);
            circleSource = new Rectangle(0, 0, 32, 32);
            circleOrigin = new Vector2(16, 16);

            arrowDestination = new Rectangle(0, 0, 80, 80);

            DayColor = new Color(255, 255, 255, 255);
            NightColor = new Color(63, 137, 255, 255);
            CurrentColor = DayColor;
        }

        public override void Update(GameTime gameTime)
        {
            float timeOfDay = GamePlayState.WorldTimer.TimeOfDay;
            rotation = -MathUtils.ToRadians(timeOfDay);

            if(timeOfDay >= 180 && timeOfDay <= 185)
            {
                float amount = Math.Min(1.0f, (timeOfDay - 180.0f) / 5.0f);
                CurrentColor = Color.Lerp(DayColor, NightColor, amount);
            }
            else if(timeOfDay >= 0 && timeOfDay <= 5)
            {
                float amount = Math.Min(1.0f, timeOfDay / 5.0f);
                CurrentColor = Color.Lerp(NightColor, DayColor, amount);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circle, circleDestination, circleSource, Color.White, rotation, circleOrigin, SpriteEffects.None, 1);
            spriteBatch.Draw(arrow, arrowDestination, Color.White);
        }

        public override bool Intersects(int x, int y)
        {
            throw new NotImplementedException();
        }

    }
}
