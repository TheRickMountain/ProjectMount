using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public enum Layer
    {
        FIRST,
        SECOND
    }

    public class Tile
    {      
        public int X { get; }
        public int Y { get; }
        public int FirstLayerId { get; set; }
        public int SecondLayerId { get; set; }
        public Entity Entity { get; set; }
        public bool IsWalkable { get; set; }
        private CollisionLayer layer;

        public Tile(int x, int y, int firstLayerId, int secondLayerId, CollisionLayer layer)
        {
            X = x;
            Y = y;
            FirstLayerId = firstLayerId;
            SecondLayerId = secondLayerId;
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
                    tiles[y * Width + x] = new Tile(x, y, TileMap.GRASS, -1, this);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= Width || y >= Height)
                throw new Exception("Выход за пределы TileMap");

            return tiles[y * Width + x];
        }

        public void SetTile(int x, int y, int firstLayerId, int secondLayerId, bool isWalkable)
        {
            Tile tile = GetTile(x, y);
            tile.FirstLayerId = firstLayerId;
            tile.SecondLayerId = secondLayerId;
            tile.IsWalkable = isWalkable;
        }

        public void SetTile(int x, int y, int id, Layer layer, bool isWalkable)
        {
            Tile tile = GetTile(x, y);
            switch(layer)
            {
                case Layer.FIRST:
                    tile.FirstLayerId = id;
                    break;
                case Layer.SECOND:
                    tile.SecondLayerId = id;
                    break;
            }
            tile.IsWalkable = isWalkable;
        }

    }
}
