using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.Entities
{
    public class Stick : Entity
    {
        private Texture2D texture;

        public Stick(Game game) : base(game, Entity.STICK)
        {
            texture = game.Content.Load<Texture2D>(@"stick");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height), Color.White);
        }
        
    }
}
