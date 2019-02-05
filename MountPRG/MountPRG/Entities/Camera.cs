using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MountPRG.TileEngine;

namespace MountPRG.Entities
{
    public enum CameraMode { Free, Follow }

    public class Camera
    {
        private Vector2 position;
        private float speed;
        private float zoom;
        private CameraMode mode;

        public Vector2 Position
        {
            get { return position; }
            private set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float Zoom
        {
            get { return zoom; }
        }

        public CameraMode CameraMode
        {
            get { return mode; }
        }

        public Camera() : this(Vector2.Zero)
        {

        }

        public Camera(Vector2 position)
        {
            speed = 50f;
            zoom = 2f;
            Position = position;
            mode = CameraMode.Follow;
        }

        public void Update(GameTime gameTime)
        {
            if (mode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputManager.KeyDown(Keys.A))
                motion.X = -speed;
            else if (InputManager.KeyDown(Keys.D))
                motion.X = speed;

            if (InputManager.KeyDown(Keys.W))
                motion.Y = -speed;
            else if (InputManager.KeyDown(Keys.S))
                motion.Y = speed;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                position += motion * speed * zoom * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(zoom) *
                Matrix.CreateTranslation(new Vector3(-(int)Position.X, -(int)Position.Y, 0f));
            }
        }

        public void LockToSprite(Player player)
        {
            position.X += (player.Position.X * zoom
                - (Game1.ScreenRectangle.Width / 2) - position.X) * .1f;
            position.Y += (player.Position.Y * zoom
                - (Game1.ScreenRectangle.Height / 2) - position.Y) * .1f;
        }

        public void ToggleCameraMode()
        {
            mode = (mode == CameraMode.Follow ? CameraMode.Free : CameraMode.Follow);
        }

        public void LockToMap(TileMap map)
        {
            position.X = MathHelper.Clamp(position.X,
                0, map.WidthInPixels * zoom - Game1.ScreenRectangle.Width);
            position.Y = MathHelper.Clamp(position.Y,
                0, map.HeightInPixels * zoom - Game1.ScreenRectangle.Height);
        }

        public void ZoomIn()
        {
            zoom += 0.25f;

            if (zoom > 2.5f)
                zoom = 2.5f;

            Vector2 newPosition = Position * zoom;
            SnapToPosition(newPosition);
        }

        public void ZoomOut()
        {
            zoom -= 0.25f;

            if (zoom < 0.5f)
                zoom = 0.5f;

            Vector2 newPosition = Position * zoom;
            SnapToPosition(newPosition);
        }

        private void SnapToPosition(Vector2 newPosition)
        {
            position.X = newPosition.X - Game1.ScreenRectangle.Width / 2;
            position.Y = newPosition.Y - Game1.ScreenRectangle.Height / 2;
        }

    }
}
