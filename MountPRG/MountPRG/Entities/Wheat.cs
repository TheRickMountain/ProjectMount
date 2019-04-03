﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Wheat : Entity
    {

        public Wheat()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.WHEAT], 16, 16, true));
            Add(new GatherableCmp(ItemDatabase.GetItemById(TileMap.WHEAT_SEED), 1));
        }

    }
}