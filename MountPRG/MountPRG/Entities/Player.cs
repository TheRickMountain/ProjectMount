using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
  
    public class Settler : Entity 
    {
        public Settler(float x, float y) : base(x, y)
        {
            Tag = "Player";
            AnimatedSprite sprite = new AnimatedSprite(ResourceBank.Sprites["characters"], 16, 16);
            sprite.Animations.Add(AnimationKey.Down, new Animation(3, 1, 16, 16, 0, 0));
            sprite.Animations.Add(AnimationKey.Left, new Animation(3, 1, 16, 16, 0, 16));
            sprite.Animations.Add(AnimationKey.Right, new Animation(3, 1, 16, 16, 0, 32));
            sprite.Animations.Add(AnimationKey.Up, new Animation(3, 1, 16, 16, 0, 48));
            sprite.Origin.Y = 6;
            sprite.IsAnimating = true;
            Add(sprite);

            Add(new PlayerController(sprite));
            Add(new Collider(12, 12));
            
        }

    }
}
