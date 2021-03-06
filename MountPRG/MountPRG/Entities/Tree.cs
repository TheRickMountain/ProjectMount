﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Tree : Entity
    {

        public Tree()
        {
            SpriteCmp sprite = new SpriteCmp(ResourceBank.Sprites["tree"]);
            sprite.Origin.X = 8;
            sprite.Origin.Y = 32;
            Add(sprite);
            Add(new Mineable(GamePlayState.ItemDatabase[Item.WOOD], 1));
            Walkable = false;
        }

    }
}
