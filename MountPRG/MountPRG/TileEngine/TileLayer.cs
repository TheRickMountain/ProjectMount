using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace MountPRG
{
    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public int Id { get; set; }
        public Entity Entity { get; set; }
        public Color Color { get; set; }
        public bool IsWalkable { get; set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            IsWalkable = true;
        }

        public float MovementCost
        {
            get { return IsWalkable ? 1.0f : 0.0f; }
        }

        public List<Tile> GetNeighbours(TileLayer tileLayer)
        {
            List<Tile> tiles = new List<Tile>();

            for(int i = X - 1; i <= X + 1; i++)
            {
                for(int j = Y - 1; j <= Y + 1; j++)
                {
                    if (i == X && j == Y)
                        continue;

                    if(i >= 0 && j >= 0 && i < tileLayer.Width && j < tileLayer.Height)
                        tiles.Add(tileLayer.GetTile(i, j));
                }
            }

            return tiles;
        }
    }

    public class TileLayer
    {

        private Tile[] tiles;

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


        public TileLayer(int width, int height, int fill)
        {
            Visible = true;

            tiles = new Tile[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Tile tile = new Tile(x, y);
                    tiles[y * width + x] = tile;
                    tile.Id = fill;
                    tile.Color = Color.White;
                }
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= width || y >= height)
                throw new Exception("Выход за пределы TileMap");

            return tiles[y * width + x];
        }

    }

}
