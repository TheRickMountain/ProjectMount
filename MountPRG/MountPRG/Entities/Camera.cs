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
        public CameraMode Mode = CameraMode.Follow;

        private Texture2D selectorTexture;
        private Rectangle selectorDest;
        private Color selectorColor;

        private bool showSelection = false;
        private Texture2D selectionTexture;
        private Rectangle selectionDest;

        public Camera() : this(Vector2.Zero)
        {

        }

        public Camera(Vector2 position)
        {
            Position = position;
            selectorTexture = ResourceBank.SelectorTexture;
            selectionTexture = ResourceBank.SelectionTexture;
            selectorColor = Color.White;

            selectorDest = new Rectangle(0, 0, selectorTexture.Width, selectorTexture.Height);
            selectionDest = new Rectangle(0, 0, selectionTexture.Width, selectionTexture.Height);
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
            Tile tile = GamePlayState.TileMap.GetTile(GetCellX(), GetCellY());
            if (tile != null)
                selectorColor = tile.IsWalkable ? Color.White : Color.Red;


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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectorTexture, selectorDest, selectorColor);
        }

        public void DrawSelection(SpriteBatch spriteBatch)
        {
            if (showSelection)
            {
                spriteBatch.Draw(selectionTexture, selectionDest, Color.White);
            }
        }

        public void SetSelection(int x, int y)
        {
            showSelection = true;
            selectionDest.X = x * TileMap.TILE_SIZE;
            selectionDest.Y = y * TileMap.TILE_SIZE;
        }

        public void RemoveSelection()
        {
            showSelection = false;
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

        public void LockToMap(TileMap tileMap)
        {
            Position.X = MathHelper.Clamp(Position.X,
                0, tileMap.WidthInPixels * Zoom - Game1.ScreenRectangle.Width);
            Position.Y = MathHelper.Clamp(Position.Y,
                0, tileMap.HeightInPixels * Zoom - Game1.ScreenRectangle.Height);
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

    }
}
