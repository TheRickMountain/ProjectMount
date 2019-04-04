using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class MyTexture
    {
        public Texture2D Texture { get; private set; }
        public Rectangle ClipRect { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MyTexture(Texture2D texture)
        {
            Texture = texture;
            ClipRect = new Rectangle(0, 0, texture.Width, texture.Height);
            Width = ClipRect.Width;
            Height = ClipRect.Height;
        }

        public MyTexture(Texture2D texture, Rectangle clipRect)
        {
            Texture = texture;
            ClipRect = clipRect;
            Width = ClipRect.Width;
            Height = ClipRect.Height;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
        {
            spriteBatch.Draw(Texture, destination, ClipRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color, 
            float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(Texture, destination, ClipRect, color, rotation, origin, effects, 1);
        }

    }
}
