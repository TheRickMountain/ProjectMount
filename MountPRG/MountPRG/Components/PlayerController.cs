using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public enum State
    {
        IDLE,
        MOVE
    }

    public class PlayerController : Component
    {
        private AnimatedSprite sprite;

        private float speed;

        private State state = State.IDLE;

        public PlayerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;
            speed = 60f;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            state = State.IDLE;

            if (InputManager.KeyDown(Keys.A))
            {
                motion.X = -1;
                sprite.CurrentAnimation = AnimationKey.Left;
                state = State.MOVE;
            }
            else if (InputManager.KeyDown(Keys.D))
            {
                motion.X = 1;
                sprite.CurrentAnimation = AnimationKey.Right;
                state = State.MOVE;
            }

            if (InputManager.KeyDown(Keys.W))
            {
                motion.Y = -1;
                sprite.CurrentAnimation = AnimationKey.Up;
                state = State.MOVE;
            }
            else if (InputManager.KeyDown(Keys.S))
            {
                motion.Y = 1;
                sprite.CurrentAnimation = AnimationKey.Down;
                state = State.MOVE;
            }

            if (state == State.IDLE)
                sprite.ResetAnimation();

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Entity.Position += motion * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //LockToMap(world.TileMap);
            }
        }

    }
}
