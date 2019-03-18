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
        public float Alpha = 1.0f;

        public Sprite(Texture2D texture, bool active)
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), texture.Width, texture.Height, active)
        {
        }

        public Sprite(Texture2D texture, int width, int height, bool active)
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), width, height, active)
        {

        }

        public Sprite(Texture2D texture, Rectangle source, int width, int height, bool active)
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

                // TODO: Исправить код смены канала Alpha при нахождении за спратом персонажей
                // Переместить в Update
                /*if(Parent.Tag != "Character" && Parent.Tag == "Tree")
                {
                    for(int i = 0; i < GamePlayState.Characters.Count; i++)
                    {
                        Sprite charSprite = GamePlayState.Characters[i].Get<Sprite>();
                        Rectangle charDest = charSprite.Destination;
                        if((charDest.Bottom > Destination.Y && charDest.Bottom < Destination.Bottom)
                            && ((charDest.X > Destination.X && charDest.X < Destination.Right)
                                || (charDest.Right > Destination.X && charDest.Right < Destination.Right)))
                        {
                            Alpha = (float)Math.Max(0.4, Alpha -= 0.05f);
                        }
                        else
                        {
                            if(Alpha < 1.0)
                                Alpha += 0.05f;
                        }
                    }
                }*/
                // TODO: Конец

                
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
