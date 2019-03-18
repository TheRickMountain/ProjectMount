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
        BUILDING
    }

    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public TileMap Tilemap { get; }
        public int GroundLayerId { get; set; }
        public int BuildingLayerId { get; set; }
        public int EdgeLayerId { get; set; }
        public Entity Entity { get; set; }
        public Entity EntityToAdd { get; set; }
        public bool Walkable { get; set; }
        public Color GroundLayerColor { get; set; }
        public Color BuildingLayerColor { get; set; }
        public Color EdgeLayerColor { get; set; }
        public bool Selected { get; set; }
        public int Stockpile { get; set; }

        public Tile(int x, int y, int groundLayerId, int buildingLayerId, TileMap tilemap)
        {
            X = x;
            Y = y;
            Tilemap = tilemap;
            GroundLayerId = groundLayerId;
            BuildingLayerId = buildingLayerId;
            EdgeLayerId = -1;
            Walkable = true;
            GroundLayerColor = Color.White;
            BuildingLayerColor = Color.White;
            Stockpile = -1;
        }

        public List<Tile> GetNeighbours(bool withDiag)
        {
            List<Tile> tiles = new List<Tile>();

            if (withDiag)
            {
                for (int i = X - 1; i <= X + 1; i++)
                {
                    for (int j = Y - 1; j <= Y + 1; j++)
                    {
                        if (i == X && j == Y)
                            continue;

                        if (i >= 0 && j >= 0 && i < Tilemap.Width && j < Tilemap.Height)
                            tiles.Add(Tilemap.GetTile(i, j));
                    }
                }
            }
            else
            {
                tiles.Add(Tilemap.GetTile(X - 1, Y));
                tiles.Add(Tilemap.GetTile(X + 1, Y));
                tiles.Add(Tilemap.GetTile(X, Y - 1));
                tiles.Add(Tilemap.GetTile(X, Y + 1));
            }

            return tiles;
        }

        public bool IsNeighbour(Tile tileToCheck)
        {
            for (int i = X - 1; i <= X + 1; i++)
            {
                for (int j = Y - 1; j <= Y + 1; j++)
                {
                    if (tileToCheck == Tilemap.GetTile(i, j))
                        return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return X + " " + Y;
        }
    }

    public class TileMap
    {
        public const int GRASS_TILE = 0;
        public const int GRASS_FLOWER_TILE = 1;
        public const int STONE_1_BLOCK = 2;
        public const int STONE_2_BLOCK = 3;
        public const int GROUND_TILE = 4;
        public const int BUSH = 5;
        public const int RASPBERRY_BUSH = 6;
        public const int BLUEBERRY_BUSH = 7;
        public const int STICK = 8;
        public const int FLINT = 9;
        public const int HAY = 10;
        public const int BERRY = 11;
        public const int WOODEN_SPEAR = 12;
        public const int FLINT_KNIFE = 13;
        public const int GRASS = 14;

        public const int STRAW_HUT_0 = 16;
        public const int STRAW_HUT_1 = 17;
        public const int STRAW_HUT_2 = 32;
        public const int STRAW_HUT_3 = 33;

        public const int TILE_SIZE = 16;

        private TileSet tileSet;

        private Tile[] tiles;

        private PathTileGraph tileGraph;

        private Texture2D selectorTexture;

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

            int tmp = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tmp = MyRandom.Range(1, 10);
                    if (tmp == 10)
                        tmp = GRASS_FLOWER_TILE;
                    else
                        tmp = GRASS_TILE;
                    
                    tiles[y * Width + x] = new Tile(x, y, tmp, -1, this);
                }
            }

            tileGraph = new PathTileGraph(this);

            selectorTexture = ResourceBank.Sprites["selector"];
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return null;

            return tiles[y * Width + x];
        }

        public void SetTile(int x, int y, int firstLayerId, int secondLayerId, bool isWalkable)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return;

            Tile tile = tiles[y * Width + x];
            tile.GroundLayerId = firstLayerId;
            tile.BuildingLayerId = secondLayerId;
            tile.Walkable = isWalkable;
        }

        public void SetTile(int x, int y, int id, Layer layer, bool isWalkable)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return;

            Tile tile = tiles[y * Width + x];
            switch(layer)
            {
                case Layer.GROUND:
                    tile.GroundLayerId = id;
                    break;
                case Layer.BUILDING:
                    tile.BuildingLayerId = id;
                    break;
            }
            tile.Walkable = isWalkable;
        }

        public bool AddEntity(int x, int y, Entity entity, bool walkable = true)
        {
            Tile tile = GetTile(x, y);

            if (tile != null)
            {
                if (tile.BuildingLayerId != -1)
                {
                    Console.WriteLine("Tile " + x + " " + y + " has block");
                    return false;
                }

                if (tile.Entity != null)
                {
                    Console.WriteLine("Tile " + x + " " + y + " has entity");
                    return false;
                }

                GamePlayState.Entities.Add(entity);
                entity.X = tile.X * TILE_SIZE;
                entity.Y = tile.Y * TILE_SIZE;
                tile.Entity = entity;
                tile.Walkable = walkable;

                return true;
            }

            return false;
        }

        public void RemoveEntity(int x, int y)
        {
            Tile tile = GetTile(x, y);

            if (tile != null)
            {
                if (tile.Entity != null)
                {
                    GamePlayState.Entities.Remove(tile.Entity);
                    tile.Entity = null;
                    tile.Walkable = true;
                }
            }
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
            int groundIndex;
            int buildingIndex;
            int edgeIndex;
            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * TILE_SIZE;

                for (int x = min.X; x < max.X; x++)
                {
                    // Сразу получаем Id двух текстур для отрисовки
                    Tile tile = GetTile(x, y);
                    groundIndex = tile.GroundLayerId;
                    buildingIndex = tile.BuildingLayerId;
                    edgeIndex = tile.EdgeLayerId;

                    destination.X = x * TILE_SIZE;

                    // -1 говорит об отсутствии текстуры
                    if (groundIndex != -1)
                    {

                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[groundIndex],
                            tile.GroundLayerColor == Color.White ? DayNightSystemGUI.CurrentColor : tile.GroundLayerColor);
                    }

                    if (buildingIndex != -1)
                    {
                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[buildingIndex],
                            tile.BuildingLayerColor == Color.White ? DayNightSystemGUI.CurrentColor : tile.BuildingLayerColor);
                    }

                    if(edgeIndex != -1)
                    {
                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[edgeIndex],
                            tile.EdgeLayerColor);
                    }

                    if (tile.Selected)
                        spriteBatch.Draw(selectorTexture, destination, Color.Yellow);
                }
            }
        }

    }
}
