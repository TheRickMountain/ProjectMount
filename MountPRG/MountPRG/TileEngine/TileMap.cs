﻿using System;
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
        EDGE,
        BUILDING
    }

    public class Tile
    {
        public int X { get; }
        public int Y { get; }
        public TileMap Tilemap { get; }
        private int groundLayerId;
        public int GroundLayerId {
            get { return groundLayerId; }
            set {
                groundLayerId = value;
                Update();
            } }
        public int EdgeLayerId { get; set; }
        public int BuildingLayerId { get; set; }
        public Entity Entity { get; set; }

        public Item Item { get; private set; }
        public int ItemCount { get; set; }
        public Item ItemToAdd { get; set; }
        public Item ItemToRemove { get; set; }

        public bool Walkable { get; set; }

        public Color GroundLayerColor { get; set; }
        public Color EdgeLayerColor { get; set; }
        public Color BuildingLayerColor { get; set; }

        public bool Selected { get; set; }
        public Area Area { get; private set; }

        public Tile(int x, int y, int groundLayerId, int buildingLayerId, TileMap tilemap)
        {
            X = x;
            Y = y;
            Tilemap = tilemap;
            GroundLayerId = groundLayerId;
            EdgeLayerId = -1;
            BuildingLayerId = buildingLayerId;
            Walkable = true;
            GroundLayerColor = Color.White;
            EdgeLayerColor = Color.White;
            BuildingLayerColor = Color.White;
            Area = new Area();
        }

        private void Update()
        {
            if(groundLayerId != TileMap.GRASS_TILE && groundLayerId != TileMap.GRASS_FLOWER_TILE)
            {
                Update(this);

                List<Tile> neighbours = GetNeighbours(false);
                for(int i = 0; i < neighbours.Count; i++)
                {
                    Tile tile = neighbours[i];
                    if (tile.groundLayerId != TileMap.GRASS_TILE && tile.groundLayerId != TileMap.GRASS_FLOWER_TILE)
                    {
                        Update(tile);
                    }
                }
            }
        }

        private void Update(Tile tile)
        {
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;

            Tile neigh = Tilemap.GetTile(tile.X - 1, tile.Y);
            if (neigh != null)
            {
                left = (neigh.groundLayerId == TileMap.GRASS_TILE || neigh.groundLayerId == TileMap.GRASS_FLOWER_TILE);
            }

            neigh = Tilemap.GetTile(tile.X + 1, tile.Y);
            if (neigh != null)
            {
                right = (neigh.groundLayerId == TileMap.GRASS_TILE || neigh.groundLayerId == TileMap.GRASS_FLOWER_TILE);
            }

            neigh = Tilemap.GetTile(tile.X, tile.Y - 1);
            if (neigh != null)
            {
                up = (neigh.groundLayerId == TileMap.GRASS_TILE || neigh.groundLayerId == TileMap.GRASS_FLOWER_TILE);
            }

            neigh = Tilemap.GetTile(tile.X, tile.Y + 1);
            if (neigh != null)
            {
                down = (neigh.groundLayerId == TileMap.GRASS_TILE || neigh.groundLayerId == TileMap.GRASS_FLOWER_TILE);
            }

            if (up && down && left && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_UDLR;
                return;
            }

            if (up && down && left)
            {
                tile.EdgeLayerId = TileMap.GRASS_UDL;
                return;
            }

            if (up && down && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_UDR;
                return;
            }

            if (up && left && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_ULR;
                return;
            }

            if (down && left && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_DLR;
                return;
            }

            if (up && down)
            {
                tile.EdgeLayerId = TileMap.GRASS_UD;
                return;
            }

            if (left && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_LR;
                return;
            }

            if (up && left)
            {
                tile.EdgeLayerId = TileMap.GRASS_UL;
                return;
            }

            if (up && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_UR;
                return;
            }

            if (down && left)
            {
                tile.EdgeLayerId = TileMap.GRASS_DL;
                return;
            }

            if (down && right)
            {
                tile.EdgeLayerId = TileMap.GRASS_DR;
                return;
            }

            if (up)
            {
                tile.EdgeLayerId = TileMap.GRASS_U;
                return;
            }

            if (down)
            {
                tile.EdgeLayerId = TileMap.GRASS_D;
                return;
            }

            if (left)
            {
                tile.EdgeLayerId = TileMap.GRASS_L;
                return;
            }

            if (right)
            {
                tile.EdgeLayerId = TileMap.GRASS_R;
                return;
            }

            tile.EdgeLayerId = -1;
        }

        
        public void AddItem(Item item, int count)
        {
            Item = item;
            ItemCount = count;
            BuildingLayerId = item.Id;
            Walkable = true;
        }

        public void RemoveItem()
        {
            Item = null;
            ItemCount = 0;
            BuildingLayerId = -1;
        }

        public void RemoveEntity()
        {
            if (Entity != null)
            {
                GamePlayState.Entities.Remove(Entity);
                Entity = null;
                Walkable = true;
            }
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
                        {
                            Tile tile = Tilemap.GetTile(i, j);
                            if (tile != null)
                                tiles.Add(tile);
                        }
                    }
                }
            }
            else
            {
                Tile tile = Tilemap.GetTile(X - 1, Y);
                if(tile != null)
                    tiles.Add(tile);

                tile = Tilemap.GetTile(X + 1, Y);
                if (tile != null)
                    tiles.Add(tile);

                tile = Tilemap.GetTile(X, Y - 1);
                if (tile != null)
                    tiles.Add(tile);

                tile = Tilemap.GetTile(X, Y + 1);
                if (tile != null)
                    tiles.Add(tile);
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

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
                return false;

            Tile tile = (Tile)obj;
            return tile.X == X && tile.Y == Y;
        }
    }

    public class TileMap
    {
        public const int GRASS_TILE = 0;
        public const int GRASS_FLOWER_TILE = 1;
        public const int STONE_1_BLOCK = 2;
        public const int STONE_2_BLOCK = 3;
        public const int DIRT_TILE = 4;
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
        public const int WOOD = 15;
        public const int STONE = 20;
        public const int WATER_FRONT_TILE = 21;
        public const int WATER_1_TILE = 22;
        public const int WATER_2_TILE = 23;
        public const int WATER_3_TILE = 24;
        public const int WATER_4_TILE = 25;
        public const int WATER_LEFT_TILE = 26;
        public const int WATER_RIGHT_TILE = 27;
        public const int FISH = 28;
        public const int WHEAT = 29;
        public const int WHEAT_SEED = 30;
        public const int FARM_TILE = 31;
        public const int WHEAT_SEED_TILE = 36;

        public const int GRASS_U = 48;
        public const int GRASS_D = 49;
        public const int GRASS_UD = 50;
        public const int GRASS_R = 51;
        public const int GRASS_L = 52;
        public const int GRASS_LR = 53;
        public const int GRASS_UDLR = 54;
        public const int GRASS_ULR = 55;
        public const int GRASS_DLR = 56;
        public const int GRASS_UDL = 57;
        public const int GRASS_UDR = 58;
        public const int GRASS_UL = 59;
        public const int GRASS_UR = 60;
        public const int GRASS_DL = 61;
        public const int GRASS_DR = 62;

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
            int edgeIndex;
            int buildingIndex;
            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * TILE_SIZE;

                for (int x = min.X; x < max.X; x++)
                {
                    // Сразу получаем Id двух текстур для отрисовки
                    Tile tile = GetTile(x, y);
                    groundIndex = tile.GroundLayerId;
                    edgeIndex = tile.EdgeLayerId;
                    buildingIndex = tile.BuildingLayerId;

                    destination.X = x * TILE_SIZE;

                    // -1 говорит об отсутствии текстуры
                    if (groundIndex != -1)
                    {

                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[groundIndex],
                            tile.GroundLayerColor);
                    }

                    if (edgeIndex != -1)
                    {
                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[edgeIndex],
                            tile.EdgeLayerColor);
                    }

                    if (buildingIndex != -1)
                    {
                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[buildingIndex],
                            tile.BuildingLayerColor);
                    }

                    if (tile.Selected)
                        spriteBatch.Draw(selectorTexture, destination, Color.Yellow);
                }
            }
        }

    }
}
