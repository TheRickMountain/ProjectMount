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
        private Rectangle selectorDestination;

        public Camera() : this(Vector2.Zero)
        {

        }

        public Camera(Vector2 position)
        {
            Position = position;
            selectorTexture = TextureBank.SelectorTexture;
            selectorDestination = new Rectangle(0, 0, selectorTexture.Width, selectorTexture.Height);
        }

        public int GetCellX()
        {
            return (int)(((InputManager.GetX() + Position.X) / Zoom) / Engine.TileWidth);
        }

        public int GetCellY()
        {
            return (int)(((InputManager.GetY() + Position.Y) / Zoom) / Engine.TileHeight);
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
            selectorDestination.X = GetCellX() * Engine.TileWidth;
            selectorDestination.Y = GetCellY() * Engine.TileHeight;

            if (Mode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputManager.KeyDown(Keys.A))
                motion.X = -Speed;
            else if (InputManager.KeyDown(Keys.D))
                motion.X = Speed;

            if (InputManager.KeyDown(Keys.W))
                motion.Y = -Speed;
            else if (InputManager.KeyDown(Keys.S))
                motion.Y = Speed;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Position += motion * Speed * Zoom * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectorTexture, selectorDestination, Color.White);
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
            Position.X += (entity.X * Zoom
                - (Game1.ScreenRectangle.Width / 2) - Position.X) * .1f;
            Position.Y += (entity.Y * Zoom
                - (Game1.ScreenRectangle.Height / 2) - Position.Y) * .1f;
        }

        public void ToggleCameraMode()
        {
            Mode = (Mode == CameraMode.Follow ? CameraMode.Free : CameraMode.Follow);
        }

        public void LockToMap(TileMap map)
        {
            Position.X = MathHelper.Clamp(Position.X,
                0, map.WidthInPixels * Zoom - Game1.ScreenRectangle.Width);
            Position.Y = MathHelper.Clamp(Position.Y,
                0, map.HeightInPixels * Zoom - Game1.ScreenRectangle.Height);
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
