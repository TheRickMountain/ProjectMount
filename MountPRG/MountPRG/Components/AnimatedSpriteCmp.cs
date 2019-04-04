using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class AnimatedSpriteCmp : Component
    {   
        public bool IsAnimating { get; set; }

        public Texture2D Texture;
        public Rectangle Destination;
        public Rectangle Source;
        public Vector2 Origin;
        public Vector2 Scale = Vector2.One;
        public float Rotation;
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;
        public float Alpha = 1.0f;

        public Dictionary<AnimationKey, Animation> Animations { get; private set; }

        public AnimationKey CurrentAnimation { get; set; }

        public AnimatedSpriteCmp(Texture2D texture, int width, int height)
            : base(true, true)
        {
            Animations = new Dictionary<AnimationKey, Animation>();
            Texture = texture;
            Destination = new Rectangle(0, 0, width, height);
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

        public bool Intersects(int x, int y)
        {
            return Destination.Contains(new Point(x, y));
        }

        public override Component Clone()
        {
            throw null;
        }
    }
}
