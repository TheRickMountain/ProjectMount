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

        List<TileLayer> mapLayers = new List<TileLayer>();

        int mapWidth;
        int mapHeight;

        TileSet tileSet;

        public TileSet TileSet
        {
            get { return tileSet; }
        }

        public CollisionLayer CollisionLayer
        {
            get;
            private set;
        }

        public int MapWidth
        {
            get { return mapWidth; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        public TileMap(TileSet tileSet, int mapWidth, int mapHeight)
        {
            this.tileSet = tileSet;

            groundLayer = new TileLayer(mapWidth, mapHeight, GRASS);

            edgeLayer = new TileLayer(mapWidth, mapHeight, -1);

            mapLayers.Add(groundLayer);
            mapLayers.Add(edgeLayer);

            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;

            CollisionLayer = new CollisionLayer(this);
        }

        public TileLayer GetGroundLayer()
        {
            return groundLayer;
        }

        public void SetGroundLayer(int x, int y, int tile, CollisionType type = CollisionType.Passable)
        {
            groundLayer.SetTile(x, y, tile);
            CollisionLayer.SetCollider(x, y, type);
        }

        public TileLayer GetEdgeLayer()
        {
            return edgeLayer;
        }

        public void SetEdgeLayer(int x, int y, int tile, CollisionType type = CollisionType.Passable)
        {
            edgeLayer.SetTile(x, y, tile);
            CollisionLayer.SetCollider(x, y, type);
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
            max.X = Math.Min(xViewPort + 1, mapWidth);
            max.Y = Math.Min(yViewPort + 1, mapHeight);

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
                        tileIndex = layer.GetTile(x, y).Id;

                        if (tileIndex == -1)
                            continue;

                        destination.X = x * Engine.TileWidth;

                        spriteBatch.Draw(
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[tileIndex],
                            layer.GetTile(x, y).Color);
                    }
                }
            }
        }
    }
}
