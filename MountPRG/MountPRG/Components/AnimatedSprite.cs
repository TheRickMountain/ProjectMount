using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class AnimatedSprite : Sprite
    {   
        public bool IsAnimating { get; set; }

        public Dictionary<AnimationKey, Animation> Animations { get; private set; }

        public AnimationKey CurrentAnimation { get; set; }


        public AnimatedSprite(Texture2D texture, int width, int height)
            : base(texture, width, height, true)
        {
            Animations = new Dictionary<AnimationKey, Animation>();
        }

        public void ResetAnimation()
        {
            Animations[CurrentAnimation].Reset();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAnimating)
                Animations[CurrentAnimation].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Parent != null)
            {
                Destination.X = (int)(Parent.X - Origin.X);
                Destination.Y = (int)(Parent.Y - Origin.Y);
                Parent.Depth = Destination.Bottom;
            }

            spriteBatch.Draw(Texture, Destination, Animations[CurrentAnimation].CurrentFrameRect,
                Color, Rotation, Vector2.Zero, Effects, 0);
        }
    }
}
