using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class TileMap
    {
        public const int GRASS = 0;
        public const int STONE_BLOCK_1 = 1;
        public const int STONE_BLOCK_2 = 2;

        TileLayer groundLayer;
        TileLayer edgeLayer;
        CollisionLayer collisionLayer;

        List<TileLayer> mapLayers = new List<TileLayer>();

        public TileSet TileSet
        {
            get;
            private set;
        }

        public int MapWidth
        {
            get;
            private set;
        }

        public int MapHeight
        {
            get;
            private set;
        }

        public int WidthInPixels
        {
            get { return MapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return MapHeight * Engine.TileHeight; }
        }

        public TileMap(TileSet tileSet, int mapWidth, int mapHeight)
        {
            TileSet = tileSet;
            MapWidth = mapWidth;
            MapHeight = mapHeight;

            groundLayer = new TileLayer(MapWidth, MapHeight, GRASS);
            edgeLayer = new TileLayer(MapWidth, MapHeight, -1);

            mapLayers.Add(groundLayer);
            mapLayers.Add(edgeLayer);

            collisionLayer = new CollisionLayer(MapWidth, MapHeight);
        }
     
        public TileLayer GetGroundLayer()
        {
            return groundLayer;
        }

        public void SetGroundLayer(int x, int y, int id, bool walkable = true)
        {
            groundLayer.SetTile(x, y, id);
            collisionLayer.GetTile(x, y).IsWalkable = walkable;
        }

        public TileLayer GetEdgeLayer()
        {
            return edgeLayer;
        }

        public void SetEdgeLayer(int x, int y, int id, bool walkable = true)
        {
            edgeLayer.SetTile(x, y, id);
            collisionLayer.GetTile(x, y).IsWalkable = walkable;
        }

        public CollisionLayer GetCollisionLayer()
        {
            return collisionLayer;
        }

        public bool AddEntity(int x, int y, Entity entity)
        {
            if (edgeLayer.GetTile(x, y) == STONE_BLOCK_1
                || groundLayer.GetTile(x, y) == STONE_BLOCK_2)
            {
                Console.WriteLine("Tile " + x + " " + y + " has block");
                return false;
            }

            if (collisionLayer.GetTile(x, y).Entity != null)
            {
                Console.WriteLine("Tile " + x + " " + y + " has entity");
                return false;
            }

            collisionLayer.GetTile(x, y).Entity = entity;

            return true;
        }

        public Entity GetEntity(int x, int y)
        {
            return collisionLayer.GetTile(x, y).Entity;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            int xCamPoint;
            int yCamPoint;

            int xViewPort;
            int yViewPort;

            Engine.VectorToCell(camera.Position.X * (1 / camera.Zoom), camera.Position.Y * (1 / camera.Zoom), 
                out xCamPoint, out yCamPoint);
            Engine.VectorToCell(
                (camera.Position.X + Game1.ScreenRectangle.Width) * (1 / camera.Zoom),
                (camera.Position.Y + Game1.ScreenRectangle.Height) * (1 / camera.Zoom), 
                out xViewPort, out yViewPort);

            Point min = new Point();
            Point max = new Point();

            min.X = Math.Max(0, xCamPoint - 1);
            min.Y = Math.Max(0, yCamPoint - 1);
            max.X = Math.Min(xViewPort + 1, MapWidth);
            max.Y = Math.Min(yViewPort + 1, MapHeight);

            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            int tileIndex;

            for(int i = 0; i < mapLayers.Count; i++)
            {
                TileLayer layer = mapLayers[i];

                if (!layer.Visible)
                    continue;

                for (int y = min.Y; y < max.Y; y++)
                {
                    destination.Y = y * Engine.TileHeight;

                    for (int x = min.X; x < max.X; x++)
                    {
                        tileIndex = layer.GetTile(x, y);

                        if (tileIndex == -1)
                            continue;

                        destination.X = x * Engine.TileWidth;

                        spriteBatch.Draw(
                            TileSet.Texture,
                            destination,
                            TileSet.SourceRectangles[tileIndex],
                            Color.White);
                    }
                }
            }
        }
    }
}
