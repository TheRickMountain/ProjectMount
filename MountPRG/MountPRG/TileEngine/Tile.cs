using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

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
        public int GroundLayerId
        {
            get { return groundLayerId; }
            set
            {
                groundLayerId = value;
                Update();
            }
        }
        public int EdgeLayerId { get; set; }
        public int BuildingLayerId { get; set; }
        public Entity Entity { get; set; }

        public Item Item { get; set; }
        public int ItemCount { get; set; }
        public Item ItemToAdd { get; set; } // прелдмет который собираются добавит
        public int ItemToAddCount { get; set; } // количество предметов которые собираются добавить
        public int ItemToRemoveCount { get; set; } // количество предметов которые собираются забрать

        private bool walkable;

        public bool Walkable
        {
            get { return walkable; }
            set
            {
                if(walkable != value)
                {
                    walkable = value;
                    for (int i = 0; i < GamePlayState.Settlers.Count; i++)
                    {
                        GamePlayState.Settlers[i].Get<SettlerControllerCmp>().RebuildPath = true;
                    }
                }
            }
        }

        public Color GroundLayerColor { get; set; }
        public Color EdgeLayerColor { get; set; }
        public Color BuildingLayerColor { get; set; }

        public bool IsSelected { get; set; }

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
            if (groundLayerId != TileMap.GRASS_TILE && groundLayerId != TileMap.GRASS_FLOWER_TILE)
            {
                Update(this);

                List<Tile> neighbours = GetNeighbours(false);
                for (int i = 0; i < neighbours.Count; i++)
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
            ItemToAdd = item;
            ItemToAddCount = count;
            BuildingLayerId = item.Id;
            Walkable = true;
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
                if (tile != null)
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

        public bool IsNeighbour(Tile tile, bool withDiag)
        {
            return  Math.Abs(X - tile.X) + Math.Abs(Y - tile.Y) == 1 
                || (withDiag && Math.Abs(X - tile.X) == 1 && Math.Abs(Y - tile.Y) == 1);
        }

        public override string ToString()
        {
            return X + " " + Y;
        }

        public bool Equals(Tile tile)
        {
            if (tile == null)
                return false;

            return tile.X == X && tile.Y == Y;
        }

    }

}
