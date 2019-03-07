using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public interface IGamePlayState : IGameState
    {

    }

    public class GamePlayState : BaseGameState, IGamePlayState
    {
        public static Camera Camera;

        public static TileMap TileMap;

        public static EntityList Entities;

        private GUIManager guiManager;

        public static Player Player;

        public static List<Entity> Characters;

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Camera = new Camera();

            TileMap = new TileMap(32, 32, new TileSet(ResourceBank.TilesetTexture));

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.SetTile(5, 4, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(5, 5, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(5, 3, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(6, 3, TileMap.STONE_BLOCK_ENTRANCE, Layer.ENTITY, false);
            TileMap.SetTile(7, 3, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(7, 5, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);

            AddEntityToTileMap(15, 10, new Chest());
            AddEntityToTileMap(16, 10, new Chest());
            AddEntityToTileMap(10, 15, new Tree());
            AddEntityToTileMap(15, 16, new Wood());
            AddEntityToTileMap(16, 16, new Wood());
            AddEntityToTileMap(17, 16, new Wood());
            AddEntityToTileMap(18, 16, new Wood());
            AddEntityToTileMap(15, 17, new Stone());
            AddEntityToTileMap(16, 17, new Stone());

            Player = new Player(Engine.ToWorldPos(15), Engine.ToWorldPos(15));
            Entities.Add(Player);
            Wolf wolf = new Wolf(Engine.ToWorldPos(27), Engine.ToWorldPos(15));
            Entities.Add(wolf);

            Characters = new List<Entity>();
            Characters.Add(Player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            Entities.UpdateList();

            Entities.Update(gameTime);

            CheckCollision(Player);

            Camera.LockToEntity(Player);
            Camera.LockToMap(TileMap);

            guiManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, Camera.Transformation);

            TileMap.Draw(GameRef.SpriteBatch, Camera);

            Camera.DrawSelection(GameRef.SpriteBatch);

            Entities.Render(GameRef.SpriteBatch);

            Camera.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            // GUI Renderer

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        public void AddEntityToTileMap(int x, int y, Entity entity)
        {
            if (TileMap.AddEntity(x, y, entity, entity.Walkable))
            {
                entity.X = Engine.ToWorldPos(x);
                entity.Y = Engine.ToWorldPos(y);
                Entities.Add(entity);
            }
        }

        public void CheckCollision(Entity entity)
        {
            int cellX = Engine.ToCellPos(Player.X + TileMap.TILE_SIZE / 2);
            int cellY = Engine.ToCellPos(Player.Y + TileMap.TILE_SIZE / 2);
            Collider collider = entity.Get<Collider>();

            for (int x = cellX - 1; x <= cellX + 1; x++)
            {
                for (int y = cellY - 1; y <= cellY + 1; y++)
                {
                    if (!TileMap.GetTile(x, y).IsWalkable)
                    {
                        float deltaX = (x * TileMap.TILE_SIZE + TileMap.TILE_SIZE / 2) - (entity.X + collider.HalfWidth);
                        float deltaY = (y * TileMap.TILE_SIZE + TileMap.TILE_SIZE / 2) - (entity.Y + collider.HalfHeight);
                        if (Math.Sqrt(deltaX * deltaX + deltaY * deltaY) <= 19)
                        {
                            float intersectX = Math.Abs(deltaX) - ((TileMap.TILE_SIZE / 2) + (collider.Width / 2));
                            float intersectY = Math.Abs(deltaY) - ((TileMap.TILE_SIZE / 2) + (collider.Height / 2));

                            if (intersectX < 0.0f && intersectY < 0.0f)
                            {

                                if (intersectX > intersectY)
                                {
                                    if (deltaX > 0.0f)
                                        entity.X += intersectX;
                                    else
                                        entity.X += -intersectX;
                                }
                                else
                                {
                                    if (deltaY > 0.0f)
                                        entity.Y += intersectY;
                                    else
                                        entity.Y += -intersectY;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}