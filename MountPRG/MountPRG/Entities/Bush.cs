using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.Entities
{
    public class Bush : Entity
    {

        private Texture2D berryTextrue;
        public bool HasBerries
        {
            get;
            private set;
        }

        public Bush(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>(@"bush");
            berryTextrue = game.Content.Load<Texture2D>(@"bush_berries");
            HasBerries = true;
            IsBush = true;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(HasBerries)
                spriteBatch.Draw(berryTextrue, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
            else
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
        }

        public Entity GetBerry()
        {
            HasBerries = false;
            return new Berry(GameRef);
        }

    }
}
