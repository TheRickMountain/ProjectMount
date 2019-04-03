using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class SpriteCmp : Component
    {
        public Texture2D Texture;
        public Rectangle Destination;
        public Rectangle Source;
        public Vector2 Origin;
        public Vector2 Scale = Vector2.One;
        public float Rotation;
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;
        public float Alpha = 1.0f;

        public SpriteCmp(Texture2D texture, bool active)
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), texture.Width, texture.Height, active)
        {
        }

        public SpriteCmp(Texture2D texture, int width, int height, bool active)
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), width, height, active)
        {

        }

        public SpriteCmp(Texture2D texture, Rectangle source, int width, int height, bool active)
            : base(active, true)
        {
            Texture = texture;
            Source = source;
            Destination = new Rectangle(0, 0, width, height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Parent != null)
            {
                Destination.X = (int)(Parent.X - Origin.X);
                Destination.Y = (int)(Parent.Y - Origin.Y);
                Parent.Depth = Destination.Bottom;   
            }

            spriteBatch.Draw(Texture, Destination, Source, Color, Rotation, Vector2.Zero, Effects, 0);
        }

        public bool Intersects(int x, int y)
        {
            return Destination.Contains(new Point(x, y));
        }

        public override Component Clone()
        {
            return new SpriteCmp(Texture, Source, Destination.Width, Destination.Height, Active);
        }
    }
}
