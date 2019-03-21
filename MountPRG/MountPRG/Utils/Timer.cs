using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class Timer
    {

        private float time;

        public Timer()
        {

        }

        public float GetTime(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            return time;
        }

        public void Reset()
        {
            time = 0;
        }

    }
}
