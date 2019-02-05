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

        private List<Entity> entities = new List<Entity>();

        private List<Collider> colliders = new List<Collider>();

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            engine = Engine.GetInstance(16, 16);
            camera = new Camera();

            TileSet tileSet = new TileSet(content.Load<Texture2D>(@"tileset"), Engine.TileWidth, Engine.TileHeight);

            TileLayer groundLayer = new TileLayer(50, 50, 0);

            TileLayer edgeLayer = new TileLayer(50, 50, -1);
            edgeLayer.Visible = true;

            TileLayer buildingLayer = new TileLayer(50, 50, -1);
            buildingLayer.Visible = false;

            tileMap = new TileMap(tileSet, groundLayer, edgeLayer, buildingLayer);

            tileMap.SetGroundLayer(5, 5, TileMap.STONE_BLOCK_1, CollisionType.Impassable);

            player = new Player(GameRef);
            player.Position.X = 8;
            player.Position.Y = 8;
            AddEntity(player);

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

            camera.LockToSprite(player);
            camera.LockToMap(tileMap);

            CheckCollision();

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
        }

        public void AddEntity(Entity entity)
        {
            if (entity.HasComponent<Collider>())
                colliders.Add(entity.GetComponent<Collider>());

            entities.Add(entity);
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
