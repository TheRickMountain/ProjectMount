﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class Hut : Entity
    {

        public Hut()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, new Rectangle(0, 16, 32, 32), 32, 32, true));
            BuildingCmp buildingCmp = new BuildingCmp(2, 2);
            buildingCmp.AddRequiredResource(ItemDatabase.GetItemById(TileMap.HAY), 4);
            buildingCmp.AddRequiredResource(ItemDatabase.GetItemById(TileMap.STICK), 4);
            Add(buildingCmp);
        }

    }
}