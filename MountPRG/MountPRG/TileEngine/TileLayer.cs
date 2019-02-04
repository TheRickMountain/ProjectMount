using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG.TileEngine
{
    public class TileLayer
    {

        private int[] tiles;

        private int width;
        private int height;

        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        public bool Visible { get; set; }

        public TileLayer()
        {
            Visible = true;
        }

        public TileLayer(int[] tiles, int width, int height)
            : this()
        {
            this.tiles = (int[])tiles.Clone();
            this.width = width;
            this.height = height;
        }

        public TileLayer(int width, int height)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[y * width + x] = 0;
        }

        public TileLayer(int width, int height, int fill)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[y * width + x] = fill;
        }

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return -1;

            if (x >= width || y >= height)
                return -1;

            return tiles[y * width + x];
        }

        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= width || y >= height)
                return;

            tiles[y * width + x] = tileIndex;
        }

    }

}
