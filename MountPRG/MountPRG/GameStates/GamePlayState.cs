using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MountPRG.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MountPRG.TileEngine;
using MountPRG.Entities;
using MountPRG.Components;
using MountPRG.GUISystem;

namespace MountPRG.GameStates
{
    public interface IGamePlayState : IGameState
    {

    }

    public class GamePlayState : BaseGameState, IGamePlayState
    {

        private Engine engine;
        private Camera camera;
        private TileMap tileMap;
        private Player player;

        private GUIManager guiManager;

        private List<Entity> entities = new List<Entity>();

        private List<Collider> colliders = new List<Collider>();

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            engine = Engine.GetInstance(16, 16);
            camera = new Camera();

            guiManager = new GUIManager(GameRef);

            tileMap = new TileMap(
                new TileSet(content.Load<Texture2D>(@"tileset"), Engine.TileWidth, Engine.TileHeight), 50, 50);

            tileMap.SetEdgeLayer(5, 4, TileMap.STONE_BLOCK_1, CollisionType.Impassable);
            tileMap.SetEdgeLayer(5, 5, TileMap.STONE_BLOCK_2, CollisionType.Impassable);
            tileMap.SetEdgeLayer(5, 3, TileMap.STONE_BLOCK_1, CollisionType.Impassable);
            tileMap.SetEdgeLayer(6, 3, TileMap.STONE_BLOCK_2, CollisionType.Impassable);

            player = new Player(GameRef);
            player.Position.X = 8 + Engine.ToWorldPosX(8);
            player.Position.Y = 8 + Engine.ToWorldPosY(10);
            AddEntity(player);


            AddEntityToWorld(new Stick(GameRef), 10, 7);
            AddEntityToWorld(new Stick(GameRef), 6, 5);
            AddEntityToWorld(new Stone(GameRef), 10, 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            foreach (Entity entity in entities)
                entity.Update(gameTime);

            player.LockToMap(tileMap);

            camera.LockToSprite(player);
            camera.LockToMap(tileMap);

            CheckCollision();

            if(InputManager.MousePressed(MouseInput.LeftButton))
            {
                Point point = MousePicker(InputManager.GetX(), InputManager.GetY());
                if (tileMap.GetEdgeLayer().HasEntity(point.X, point.Y))
                {
                    Entity entity = tileMap.GetEdgeLayer().GetEntity(point.X, point.Y);
                    guiManager.AddItem(entity);
                    entities.Remove(entity);
                    tileMap.GetEdgeLayer().RemoveEntity(point.X, point.Y); 
                }
            }

            guiManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, camera.Transformation);

            tileMap.Draw(GameRef.SpriteBatch, camera);

            foreach (Entity entity in entities)
                entity.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        public void AddEntity(Entity entity)
        {
            if (entity.HasComponent<Collider>())
                colliders.Add(entity.GetComponent<Collider>());

            entities.Add(entity);
        }

        public void AddEntityToWorld(Entity entity, int x, int y)
        {
            tileMap.GetEdgeLayer().SetEntity(x, y, entity);

            entity.Position.X = Engine.ToWorldPosX(x);
            entity.Position.Y = Engine.ToWorldPosX(y);

            Console.WriteLine(tileMap.GetEdgeLayer().GetEntity(x, y));

            entities.Add(entity);
        }

        public Point MousePicker(int x, int y)
        {
            return new Point(
                (int)(((x + camera.Position.X) / camera.Zoom) / Engine.TileWidth), 
                (int)(((y + camera.Position.Y) / camera.Zoom) / Engine.TileHeight));
        }

        public void CheckCollision()
        {
            foreach (Collider collider in colliders)
            {
                Entity entity = collider.Entity;

                int cellX;
                int cellY;
                Engine.VectorToCell(entity.Position.X, entity.Position.Y, out cellX, out cellY);

                for (int x = cellX - 1; x <= cellX + 1; x++)
                {
                    for (int y = cellY - 1; y <= cellY + 1; y++)
                    {
                        switch (tileMap.CollisionLayer.GetCollider(x, y))
                        {
                            case (int)CollisionType.None:
                            case (int)CollisionType.Passable:
                                continue;
                        }

                        float deltaX = (x * Engine.TileWidth + Engine.TileWidth / 2) - (entity.Position.X + collider.OffsetX);
                        float deltaY = (y * Engine.TileHeight + Engine.TileHeight / 2) - (entity.Position.Y + collider.OffsetY);

                        float intersectX = Math.Abs(deltaX) - ((Engine.TileWidth / 2) + (collider.Width / 2));
                        float intersectY = Math.Abs(deltaY) - ((Engine.TileHeight / 2) + (collider.Height / 2));

                        if (intersectX < 0.0f && intersectY < 0.0f)
                        {

                            if (intersectX > intersectY)
                            {
                                if (deltaX > 0.0f)
                                    entity.Position += new Vector2(intersectX, 0);
                                else
                                    entity.Position += new Vector2(-intersectX, 0);
                            }
                            else
                            {
                                if (deltaY > 0.0f)
                                    entity.Position += new Vector2(0, intersectY);
                                else
                                    entity.Position += new Vector2(0, -intersectY);
                            }
                        }
                    }
                }
            }

        }
    }
}
