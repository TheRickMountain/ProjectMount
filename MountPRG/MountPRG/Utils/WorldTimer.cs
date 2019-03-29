using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class WorldTimer
    {

        private float currentTime;

        public float TimeOfDay
        {
            get; private set;
        }

        public WorldTimer()
        {

        }

        public void Update(GameTime gameTime)
        {
            currentTime -= (float)(gameTime.ElapsedGameTime.TotalSeconds * 0.05);
            float timeOfDay = -MathUtils.ToDegrees(currentTime);

            if (timeOfDay >= 360)
                currentTime = 0;

            TimeOfDay = timeOfDay;
        }
    }
}
