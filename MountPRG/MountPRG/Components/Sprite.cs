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
            : base(active, true)
        {
            Texture = texture;
            Destination = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public override void Update(float dt)
        {

            Destination.X = (int)Entity.X;
            Destination.Y = (int)Entity.Y;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Destination, Source, Color, Rotation, Origin, Effects, 0);
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
