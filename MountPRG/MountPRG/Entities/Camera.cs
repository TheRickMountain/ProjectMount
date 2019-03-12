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
            return (int)(((InputManager.GetX() + Position.X) / Zoom) / TileMap.TILE_SIZE);
        }

        public int GetCellY()
        {
            return (int)(((InputManager.GetY() + Position.Y) / Zoom) / TileMap.TILE_SIZE);
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

            if (!GUIManager.ActionPanelGUI.MouseOnUI)
            {
                if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                {
                    switch (GUIManager.ActionPanelGUI.CurrentJobType)
                    {
                        case JobType.NONE:
                            Console.WriteLine("None");
                            break;
                        case JobType.GATHER:
                            {
                                // Если объект на сбор уже отправлен, не выделять еще раз
                                if (!SelectedTile.Selected)
                                {
                                    Entity entity = SelectedTile.Entity;
                                    if (entity != null && entity.Has<Gatherable>())
                                    {
                                        SelectedTile.Selected = true;
                                        GamePlayState.JobSystem.Enqueue(new Job(SelectedTile, JobType.GATHER));
                                    }
                                }
                            }
                            break;
                        case JobType.CUT:
                            Console.WriteLine("Cut");
                            break;
                        case JobType.MINE:
                            Console.WriteLine("Mine");
                            break;
                        case JobType.BUILD:
                            Console.WriteLine("Build");
                            break;
                        case JobType.STORAGE:
                            firstSelectedTile = lastSelectedTile = SelectedTile;
                            break;
                    }
                }

                if (GUIManager.ActionPanelGUI.CurrentJobType == JobType.STORAGE && firstSelectedTile != null)
                {
                    if (InputManager.GetMouseButton(MouseInput.LeftButton))
                    {
                        SelectArea(firstSelectedTile, lastSelectedTile, Color.White);
                        SelectArea(firstSelectedTile, SelectedTile, Color.LightPink);
                        lastSelectedTile = SelectedTile;
                    }

                    if (InputManager.GetMouseButtonReleased(MouseInput.LeftButton))
                    {
                        Console.WriteLine("Area selected");
                    }
                }
            }
            

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

            GUIManager.ActionPanelGUI.MouseOnUI = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectorTexture, selectorDest, selectorColor);
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

    }
}
