using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public interface IGUI
    {


        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

    }
}
