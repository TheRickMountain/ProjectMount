using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace MountPRG
{
    public class TileLayer
    {

        private int[] tiles;

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public bool Visible { get; set; }


        public TileLayer(int width, int height, int fill)
        {
            Visible = true;

            tiles = new int[height * width];
            Width = width;
            Height = height;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    tiles[y * Width + x] = fill;
        }

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= Width || y >= Height)
                throw new Exception("Выход за пределы TileMap");

            return tiles[y * Width + x];
        }

        public void SetTile(int x, int y, int id)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= Width || y >= Height)
                throw new Exception("Выход за пределы TileMap");

            tiles[y * Width + x] = id;
        }

    }

}
