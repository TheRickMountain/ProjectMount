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
        GROUND,
        ENTITY
    }

    public class Tile
    {      
        public int X { get; }
        public int Y { get; }
        public int GroundLayerId { get; set; }
        public int EntityLayerId { get; set; }
        public Entity Entity { get; set; }
        public bool IsWalkable { get; set; }
        private TileMap tilemap;

        public Tile(int x, int y, int firstLayerId, int secondLayerId, TileMap tilemap)
        {
            X = x;
            Y = y;
            GroundLayerId = firstLayerId;
            EntityLayerId = secondLayerId;
            this.tilemap = tilemap;
            IsWalkable = true;
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

                    if (i >= 0 && j >= 0 && i < tilemap.Width && j < tilemap.Height)
                        tiles.Add(tilemap.GetTile(i, j));
                }
            }

            return tiles;
        }

        public override string ToString()
        {
            return X + " " + Y;
        }
    }

    public class TileMap
    {
        public const int GRASS = 0;
        public const int STONE_BLOCK_1 = 1;
        public const int STONE_BLOCK_2 = 2;

        public const int TILE_SIZE = 16;

        private TileSet tileSet;

        private Tile[] tiles;

        private PathTileGraph tileGraph;

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

        public PathTileGraph GetTileGraph()
        {
            return tileGraph;
        }

        public TileMap(int width, int height, TileSet tileSet)
        {
            this.tileSet = tileSet;
            Width = width;
            Height = height;

            tiles = new Tile[height * width];
            
            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                    tiles[y * Width + x] = new Tile(x, y, GRASS, -1, this);

            tileGraph = new PathTileGraph(this);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                Console.WriteLine("Выход за пределы карты");
                return null;
            }

            return tiles[y * Width + x];
        }

        public void SetTile(int x, int y, int firstLayerId, int secondLayerId, bool isWalkable)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                Console.WriteLine("Выход за пределы карты");
                return;
            }

            Tile tile = tiles[y * Width + x];
            tile.GroundLayerId = firstLayerId;
            tile.EntityLayerId = secondLayerId;
            tile.IsWalkable = isWalkable;
        }

        public void SetTile(int x, int y, int id, Layer layer, bool isWalkable)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                Console.WriteLine("Выход за пределы карты");
                return;
            }

            Tile tile = tiles[y * Width + x];
            switch(layer)
            {
                case Layer.GROUND:
                    tile.GroundLayerId = id;
                    break;
                case Layer.ENTITY:
                    tile.EntityLayerId = id;
                    break;
            }
            tile.IsWalkable = isWalkable;
        }

        public bool AddEntity(int x, int y, Entity entity, bool walkable = true)
        {
            Tile tile = GetTile(x, y);

            if (tile != null)
            {
                if (tile.EntityLayerId != -1)
                {
                    Console.WriteLine("Tile " + x + " " + y + " has block");
                    return false;
                }

                if (tile.Entity != null)
                {
                    Console.WriteLine("Tile " + x + " " + y + " has entity");
                    return false;
                }

                tile.Entity = entity;
                tile.IsWalkable = walkable;

                return true;
            }

            return false;
        }

        public Entity GetEntity(int x, int y)
        {
            Tile tile = GetTile(x, y);

            if (tile != null)
                return tile.Entity;

            return null;
        }

        public int WidthInPixels
        {
            get { return Width * TILE_SIZE; }
        }

        public int HeightInPixels
        {
            get { return Height * TILE_SIZE; }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            int xCamPoint = Engine.ToCellPos(camera.Position.X * (1 / camera.Zoom));
            int yCamPoint = Engine.ToCellPos(camera.Position.Y * (1 / camera.Zoom));

            int xViewPort = Engine.ToCellPos((camera.Position.X + Game1.ScreenRectangle.Width) * (1 / camera.Zoom));
            int yViewPort = Engine.ToCellPos((camera.Position.Y + Game1.ScreenRectangle.Height) * (1 / camera.Zoom));

            Point min = new Point();
            Point max = new Point();

            min.X = Math.Max(0, xCamPoint - 1);
            min.Y = Math.Max(0, yCamPoint - 1);
            max.X = Math.Min(xViewPort + 1, Width);
            max.Y = Math.Min(yViewPort + 1, Height);

            Rectangle destination = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            int firstIndex;
            int secondIndex;

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * TILE_SIZE;

                for (int x = min.X; x < max.X; x++)
                {
                    // Сразу получаем Id двух текстур для отрисовки
                    firstIndex = GetTile(x, y).GroundLayerId;
                    secondIndex = GetTile(x, y).EntityLayerId;

                    destination.X = x * TILE_SIZE;

                    // -1 говорит об отсутствии текстуры
                    if (firstIndex != -1)
                    {

                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[firstIndex],
                            TimeSystemGUI.CurrentColor);
                    }

                    if (secondIndex != -1)
                    {
                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[secondIndex],
                            TimeSystemGUI.CurrentColor);
                    }
                }
            }
        }

    }
}
