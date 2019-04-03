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

        public float TimeOfDay
        {
            get; private set;
        }

        public WorldTimer()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            TimeOfDay += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeOfDay >= 360)
                TimeOfDay = 0;
            
        }

        public void SetTime(float time)
        {
            TimeOfDay = MathHelper.Clamp(time, 0, 359);
        }
    }
}
