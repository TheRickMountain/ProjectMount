using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Campfire : Entity
    {
        public Campfire()
        {
            AnimatedSprite sprite = new AnimatedSprite(ResourceBank.Sprites["campfire"], 16, 24);
            sprite.Animations.Add(AnimationKey.Down, new Animation(5, 0, 16, 24, 0, 0));
            sprite.IsAnimating = true;
            sprite.Origin.Y = 8;
            sprite.Shaded = false;
            Add(sprite);
            Walkable = false;
        }

    }
}
