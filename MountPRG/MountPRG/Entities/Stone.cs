using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.Entities
{
    public class Stone : Entity
    {

        public Stone(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>(@"stone");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
        }

    }
}
