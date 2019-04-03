﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class Workbench : Entity
    {

        public Workbench()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, new Rectangle(32, 16, 32, 32), 32, 32, true));
            Add(new BuildingCmp(2, 2));
        }

    }
}
