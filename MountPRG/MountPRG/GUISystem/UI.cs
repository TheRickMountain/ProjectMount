using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public abstract class UI
    {
        public bool Active;
        public bool Visible;

        public UI()
        {
            Active = true;
            Visible = true;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public void Enable()
        {
            Active = true;
            Visible = true;
        }

        public void Disable()
        {
            Active = false;
            Visible = false;
        }

        public abstract bool Intersects(int x, int y);

    }
}
