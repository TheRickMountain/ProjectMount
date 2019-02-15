using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
  
    public class Player : Entity
    {
        public Player(Vector2 position) : base(position)
        {
            AnimatedSprite sprite = new AnimatedSprite(TextureBank.CharactersTexture, 16, 16);
            sprite.Animations.Add(AnimationKey.Down, new Animation(3, 1, 16, 16, 0, 0));
            sprite.Animations.Add(AnimationKey.Left, new Animation(3, 1, 16, 16, 0, 16));
            sprite.Animations.Add(AnimationKey.Right, new Animation(3, 1, 16, 16, 0, 32));
            sprite.Animations.Add(AnimationKey.Up, new Animation(3, 1, 16, 16, 0, 48));
            sprite.Origin.X = 8;
            sprite.Origin.Y = 12;
            sprite.IsAnimating = true;
            Add(sprite);
            Add(new PlayerController(sprite));
        }

    }
}
