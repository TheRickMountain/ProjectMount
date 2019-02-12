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
        private static Engine instance;

        public static int TileWidth
        {
            get;
            private set;
        }

        public static int TileHeight
        {
            get;
            private set;
        }


        private Engine(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public static Engine GetInstance(int tileWidth, int tileHeight)
        {
            if (instance == null)
                instance = new Engine(tileWidth, tileHeight);
            return instance;
        }

        public static void VectorToCell(float xPos, float yPos, out int xCell, out int yCell)
        {
            xCell = (int)xPos / TileWidth;
            yCell = (int)yPos / TileHeight;
        }

        public static int ToWorldPosX(int x)
        {
            return x * TileWidth;
        }

        public static int ToWorldPosY(int y)
        {
            return y * TileHeight;
        }

    }
}
