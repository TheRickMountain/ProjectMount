using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MountPRG.Components;
using MountPRG.TileEngine;

namespace MountPRG.Entities
{
    public class Player : Entity
    {

        
        private Texture2D texture;
        private float speed;
        private Vector2 origin;


        public Player(Game game) : base(game, -1)
        {
            Position = new Vector2(0, 0);
            texture = game.Content.Load<Texture2D>(@"human");
            speed = 60;
            origin = new Vector2(8, 8);
            AddComponent(new Collider(16, 8, 0, 4));
        }

        

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if (InputManager.KeyDown(Keys.A))
                motion.X = -1;
            else if (InputManager.KeyDown(Keys.D))

                motion.X = 1;

            if (InputManager.KeyDown(Keys.W))
                motion.Y = -1;
            else if (InputManager.KeyDown(Keys.S))
                motion.Y = 1;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Position += motion * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, 
                new Rectangle((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), texture.Width, texture.Height), Color.White);
        }

        public void LockToMap(TileMap map)
        {
            Position.X = MathHelper.Clamp(Position.X, origin.X, map.WidthInPixels - origin.X);
            Position.Y = MathHelper.Clamp(Position.Y, origin.Y, map.HeightInPixels - (texture.Width - origin.Y));
        }
    }
}
