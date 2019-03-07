using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public enum PlayerState
    {
        IDLE,
        MOVE
    }

    public class PlayerController : Component
    {
        private AnimatedSprite sprite;

        private float speed;

        private PlayerState playerState = PlayerState.IDLE;

        private bool inInventory;

        public PlayerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;
        }

        public override void Initialize()
        {
            speed = 60f;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyDown(Keys.Tab))
            {
                inInventory = !inInventory;
                GUIManager.InventoryGUI.Active = inInventory;
            }

            if(!inInventory)
            {
                MotionUpdate(gameTime);
            }
        }

        private void MotionUpdate(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            playerState = PlayerState.IDLE;

            if (InputManager.GetKey(Keys.A))
            {
                motion.X = -1;
                sprite.CurrentAnimation = AnimationKey.Left;
                playerState = PlayerState.MOVE;
            }
            else if (InputManager.GetKey(Keys.D))
            {
                motion.X = 1;
                sprite.CurrentAnimation = AnimationKey.Right;
                playerState = PlayerState.MOVE;
            }

            if (InputManager.GetKey(Keys.W))
            {
                motion.Y = -1;
                sprite.CurrentAnimation = AnimationKey.Up;
                playerState = PlayerState.MOVE;
            }
            else if (InputManager.GetKey(Keys.S))
            {
                motion.Y = 1;
                sprite.CurrentAnimation = AnimationKey.Down;
                playerState = PlayerState.MOVE;
            }

            if (playerState == PlayerState.IDLE)
                sprite.ResetAnimation();

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Parent.X += motion.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Parent.Y += motion.Y * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //LockToMap(world.TileMap);
            }
        }
    }
}
