using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public enum CameraMode { Free, Follow }

    public class Camera
    {
        public Vector2 Position;

        public float Speed = 150f;
        public float Zoom = 2f;
        public CameraMode Mode = CameraMode.Free;

        private Texture2D selectorTexture;
        private Rectangle selectorDest;
        private Color selectorColor;

        public Tile SelectedTile
        {
            get; private set;
        }

        private Tile firstSelectedTile;
        private Tile lastSelectedTile;

        public Camera() : this(Vector2.Zero)
        {

        }

        public Camera(Vector2 position)
        {
            Position = position;
            selectorTexture = ResourceBank.Sprites["selector"];
            selectorColor = Color.White;

            selectorDest = new Rectangle(0, 0, selectorTexture.Width, selectorTexture.Height);
        }

        public int GetCellX()
        {
            int cellX = (int)(((InputManager.GetX() + Position.X) / Zoom) / TileMap.TILE_SIZE);
            if (cellX >= GamePlayState.TileMap.Width)
                return GamePlayState.TileMap.Width - 1;
            else if (cellX < 0)
                return 0;

            return cellX;
        }

        public int GetCellY()
        {
            int cellY = (int)(((InputManager.GetY() + Position.Y) / Zoom) / TileMap.TILE_SIZE);
            if (cellY >= GamePlayState.TileMap.Height)
                return GamePlayState.TileMap.Height - 1;
            else if (cellY < 0)
                return 0;

            return cellY;
        }

        public int GetX()
        {
            return (int)((InputManager.GetX() + Position.X) / Zoom);
        }

        public int GetY()
        {
            return (int)((InputManager.GetY() + Position.Y) / Zoom);
        }

        public void Update(GameTime gameTime)
        {
            selectorDest.X = GetCellX() * TileMap.TILE_SIZE;
            selectorDest.Y = GetCellY() * TileMap.TILE_SIZE;
            SelectedTile = GamePlayState.TileMap.GetTile(GetCellX(), GetCellY());

            if (!GUIManager.MouseOnUI)
            {
                switch (GUIManager.ActionPanelGUI.CurrentJobType)
                {
                    case JobType.NONE:
                        {
                            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                            {
                                if(SelectedTile.Stockpile > -1)
                                {
                                    GUIManager.StockpileGUI.Open(GamePlayState.StockpileList.Get(SelectedTile.Stockpile));
                                }
                            }
                        }
                        break;
                    case JobType.GATHER:
                        MakeGatheringJob();
                        break;
                    case JobType.CUT:
                        //Console.WriteLine("Cut");
                        break;
                    case JobType.MINE:
                        //Console.WriteLine("Mine");
                        break;
                    case JobType.BUILD:
                        //Console.WriteLine("Build");
                        break;
                    case JobType.STOCKPILE:
                        MakeStockpile();
                        break;
                }
            }


            MovementUpdate(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectorTexture, selectorDest, selectorColor);
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (Mode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputManager.GetKey(Keys.A))
                motion.X = -Speed;
            else if (InputManager.GetKey(Keys.D))
                motion.X = Speed;

            if (InputManager.GetKey(Keys.W))
                motion.Y = -Speed;
            else if (InputManager.GetKey(Keys.S))
                motion.Y = Speed;


            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Position += motion * Speed * Zoom * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(-(int)Position.X, -(int)Position.Y, 0f));
            }
        }

        public void LockToEntity(Entity entity)
        {
            Position.X += ((entity.X + TileMap.TILE_SIZE / 2) * Zoom - (Game1.ScreenRectangle.Width / 2) - Position.X) * .1f;
            Position.Y += ((entity.Y + TileMap.TILE_SIZE / 2) * Zoom - (Game1.ScreenRectangle.Height / 2) - Position.Y) * .1f;
        }

        public void ToggleCameraMode()
        {
            Mode = (Mode == CameraMode.Follow ? CameraMode.Free : CameraMode.Follow);
        }       

        public void ZoomIn()
        {
            Zoom += 0.25f;

            if (Zoom > 2.5f)
                Zoom = 2.5f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }

        public void ZoomOut()
        {
            Zoom -= 0.25f;

            if (Zoom < 0.5f)
                Zoom = 0.5f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }

        private void SnapToPosition(Vector2 newPosition)
        {
            Position.X = newPosition.X - Game1.ScreenRectangle.Width / 2;
            Position.Y = newPosition.Y - Game1.ScreenRectangle.Height / 2;
        }

        private void MakeStockpile()
        {
            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                firstSelectedTile = lastSelectedTile = SelectedTile;
            }

            if (InputManager.GetMouseButton(MouseInput.LeftButton))
            {
                SelectArea(firstSelectedTile, lastSelectedTile, Color.White);

                int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                if (width < 2 || width > 5 || height < 2 || height > 5)
                {
                    SelectArea(firstSelectedTile, SelectedTile, Color.IndianRed);
                }
                else
                {
                    SelectArea(firstSelectedTile, SelectedTile, Color.LightGreen);
                }

                lastSelectedTile = SelectedTile;
            }

            if (InputManager.GetMouseButtonReleased(MouseInput.LeftButton))
            {
                int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                if (width >= 2 && width <= 5 && height >= 2 && height <= 5)
                {
                    GamePlayState.StockpileList.Add(GetAreaTiles(firstSelectedTile, SelectedTile));
                }

                SelectArea(firstSelectedTile, SelectedTile, Color.White);
            }
        }

        private void SelectArea(Tile firstTile, Tile lastTile, Color color)
        {
            int firstX = firstTile.X;
            int firstY = firstTile.Y;

            int lastX = lastTile.X;
            int lastY = lastTile.Y;

            if (firstX > lastX)
                MathUtils.Replace(ref firstX, ref lastX);

            if (firstY > lastY)
                MathUtils.Replace(ref firstY, ref lastY);

            for (int x = firstX; x <= lastX; x++)
            {
                for (int y = firstY; y <= lastY; y++)
                {
                    GamePlayState.TileMap.GetTile(x, y).Color = color;
                }
            }
        }

        private Tile[,] GetAreaTiles(Tile firstTile, Tile lastTile)
        {
            int firstX = firstTile.X;
            int firstY = firstTile.Y;

            int lastX = lastTile.X;
            int lastY = lastTile.Y;

            Tile[,] tiles = new Tile[Math.Abs(firstX - lastX) + 1, Math.Abs(firstY - lastY) + 1];

            if (firstX > lastX)
                MathUtils.Replace(ref firstX, ref lastX);

            if (firstY > lastY)
                MathUtils.Replace(ref firstY, ref lastY);

            for (int x = firstX; x <= lastX; x++)
            {
                for (int y = firstY; y <= lastY; y++)
                {
                    Tile tile = GamePlayState.TileMap.GetTile(x, y);
                    tile.GroundLayerId = TileMap.GROUND;
                    tiles[x - firstX, y - firstY] = tile;
                }
            }

            return tiles;
        }

        private void MakeGatheringJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    Entity entity = SelectedTile.Entity;
                    if (entity != null && entity.Has<Gatherable>())
                    {
                        SelectedTile.Selected = true;
                        GamePlayState.JobSystem.Add(new Job(SelectedTile, JobType.GATHER));
                    }
                }
            }
        }
    }
}
