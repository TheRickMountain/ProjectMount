using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MountPRG.Entities;

namespace MountPRG.TileEngine
{
    public class TileMap
    {
        TileLayer groundLayer;
        TileLayer edgeLayer;
        TileLayer buildingLayer;

        List<TileLayer> mapLayers = new List<TileLayer>();

        CollisionLayer collisionLayer;

        int mapWidth;
        int mapHeight;

        TileSet tileSet;

        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        public TileLayer GroundLayer
        {
            get { return groundLayer; }
            set { groundLayer = value; }
        }

        public TileLayer EdgeLayer
        {
            get { return edgeLayer; }
            set { edgeLayer = value; }
        }

        public TileLayer BuildingLayer
        {
            get { return buildingLayer; }
            set { buildingLayer = value; }
        }

        public CollisionLayer CollisionLayer
        {
            get { return collisionLayer; }
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

        public TileMap(TileSet tileSet, TileLayer groundLayer, TileLayer edgeLayer, TileLayer buildingLayer)
        {
            this.tileSet = tileSet;

            this.groundLayer = groundLayer;
            this.edgeLayer = edgeLayer;
            this.buildingLayer = buildingLayer;

            mapLayers.Add(groundLayer);
            mapLayers.Add(edgeLayer);
            mapLayers.Add(buildingLayer);

            mapWidth = groundLayer.Width;
            mapHeight = groundLayer.Height;

            collisionLayer = new CollisionLayer(this);
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

            foreach (TileLayer layer in mapLayers)
            {
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
                            tileSet.Texture,
                            destination,
                            tileSet.SourceRectangles[tileIndex],
                            Color.White);
                    }
                }
            }
        }
    }
}
