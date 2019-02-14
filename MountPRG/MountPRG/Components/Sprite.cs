using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Sprite : Component
    {
        public Texture2D Texture;
        public Rectangle Destination;
        public Rectangle Source;
        public Vector2 Origin;
        public Vector2 Scale = Vector2.One;
        public float Rotation;
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;

        public Sprite(Texture2D texture, bool active)
            : this(texture, texture.Width, texture.Height, active)
        {
        }

        public Sprite(Texture2D texture, int width, int height, bool active)
            : base(active, true)
        {
            Texture = texture;
            Destination = new Rectangle(0, 0, width, height);
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Entity != null)
            {
                Destination.X = (int)(Entity.X - Origin.X);
                Destination.Y = (int)(Entity.Y - Origin.Y);
            }
            spriteBatch.Draw(Texture, Destination, Source, Color, Rotation, Vector2.Zero, Effects, 0);
        }

        public bool Intersects(int x, int y)
        {
            if (x >= Destination.X && x <= Destination.Right
                && y >= Destination.Y && y <= Destination.Bottom)
                return true;

            return false;
        }

    }
}
