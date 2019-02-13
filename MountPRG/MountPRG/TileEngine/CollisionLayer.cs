using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public Entity Entity { get; set; }
        public bool IsWalkable { get; set; }
        private CollisionLayer layer;

        public Tile(int x, int y, CollisionLayer layer)
        {
            X = x;
            Y = y;
            this.layer = layer;
            IsWalkable = true;
        }

        public float MovementCost
        {
            get { return IsWalkable ? 1.0f : 0.0f; }
        }

        public List<Tile> GetNeighbours()
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = X - 1; i <= X + 1; i++)
            {
                for (int j = Y - 1; j <= Y + 1; j++)
                {
                    if (i == X && j == Y)
                        continue;

                    if (i >= 0 && j >= 0 && i < layer.Width && j < layer.Height)
                        tiles.Add(layer.GetTile(i, j));
                }
            }

            return tiles;
        }
    }

    public class CollisionLayer
    {
        private Tile[] tiles;

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

        public CollisionLayer(int width, int height)
        {
            tiles = new Tile[height * width];
            Width = width;
            Height = height;
            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                    tiles[y * Width + x] = new Tile(x, y, this);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= Width || y >= Height)
                throw new Exception("Выход за пределы TileMap");

            return tiles[y * Width + x];
        }
    }
}
