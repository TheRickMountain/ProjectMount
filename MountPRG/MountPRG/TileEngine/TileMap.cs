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

        CollisionLayer collisionLayer;

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

            collisionLayer = new CollisionLayer(MapWidth, MapHeight);
        }

        public CollisionLayer GetCollisionLayer()
        {
            return collisionLayer;
        }

        public bool AddEntity(int x, int y, Entity entity)
        {
            Tile tile = collisionLayer.GetTile(x, y);
            if(tile.SecondLayerId != -1)
            {
                Console.WriteLine("Tile " + x + " " + y + " has block");
                return false;
            }

            if (tile.Entity != null)
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

            // Будет отображено только то что входит в область видимости камеры
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
            int firstIndex;
            int secondIndex;

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    // Сразу получаем Id двух текстур для отрисовки
                    firstIndex = collisionLayer.GetTile(x, y).FirstLayerId;
                    secondIndex = collisionLayer.GetTile(x, y).SecondLayerId;

                    destination.X = x * Engine.TileWidth;

                    // -1 говорит об отсутствии текстуры
                    if (firstIndex != -1)
                    {

                        spriteBatch.Draw(
                            TileSet.Texture,
                            destination,
                            TileSet.SourceRectangles[firstIndex],
                            Color.White);
                    }

                    if(secondIndex != -1)
                    {
                        spriteBatch.Draw(
                            TileSet.Texture,
                            destination,
                            TileSet.SourceRectangles[secondIndex],
                            Color.White);
                    }
                }
            }
        }
    }
}
