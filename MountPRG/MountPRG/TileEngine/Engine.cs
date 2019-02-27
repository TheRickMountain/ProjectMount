using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Engine
    {
        public static int ToCellPos(int value)
        {
            return value / TileMap.TILE_SIZE;
        }

        public static int ToCellPos(float value)
        {
            return (int)value / TileMap.TILE_SIZE;
        }

        public static int ToWorldPos(int value)
        {
            return value * TileMap.TILE_SIZE;
        }
    }
}
