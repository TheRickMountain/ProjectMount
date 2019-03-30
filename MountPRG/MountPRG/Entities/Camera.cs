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
    public class Camera
    {
        public Vector2 Position;

        public float Speed = 150f;
        public float Zoom = 2f;

        public Camera() : this(Vector2.Zero)
        {

        }

        public Camera(Vector2 position)
        {
            Position = position;
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

        //public void LockToEntity(Entity entity)
        //{
        //    Position.X += ((entity.X + TileMap.TILE_SIZE / 2) * Zoom - (Game1.ScreenRectangle.Width / 2) - Position.X) * .1f;
        //    Position.Y += ((entity.Y + TileMap.TILE_SIZE / 2) * Zoom - (Game1.ScreenRectangle.Height / 2) - Position.Y) * .1f;
        //}     

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
