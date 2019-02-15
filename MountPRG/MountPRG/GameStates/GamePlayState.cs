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
       private bool paused;
        private Camera camera;

        public static TileMap TileMap;
        public PathTileGraph TileGraph;

        private EntityList entities;

        private GUIManager guiManager;

        private Player player;

        public GamePlayState(Game game) : base(game)
        {
            entities = new EntityList();
        }

        public override void Initialize()
        {
            Engine engine = Engine.GetInstance(16, 16);
            camera = new Camera();

            TileMap = new TileMap(
                new TileSet(TextureBank.TilesetTexture, Engine.TileWidth, Engine.TileHeight), 50, 50);

            TileMap.GetCollisionLayer().SetTile(5, 4, TileMap.STONE_BLOCK_1, Layer.SECOND, false);
            TileMap.GetCollisionLayer().SetTile(5, 5, TileMap.STONE_BLOCK_2, Layer.SECOND, false);
            TileMap.GetCollisionLayer().SetTile(5, 3, TileMap.STONE_BLOCK_1, Layer.SECOND, false);
            TileMap.GetCollisionLayer().SetTile(6, 3, TileMap.STONE_BLOCK_2, Layer.SECOND, false);

            TileGraph = new PathTileGraph(TileMap.GetCollisionLayer());

            AddEntityToTileMap(8, 5, new Wood());
            AddEntityToTileMap(15, 10, new Chest());
            AddEntityToTileMap(10, 15, new Tree());

            player = new Player(new Vector2(Engine.ToWorldPosX(1), Engine.ToWorldPosY(5)));
            entities.Add(player);

            guiManager = new GUIManager(GameRef);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            entities.UpdateList();

            if (!paused)
                entities.Update(gameTime);

            if (InputManager.MousePressed(MouseInput.LeftButton))
            {
                Entity selectedEntity = TileMap.GetEntity(camera.GetCellX(), camera.GetCellY());
                if (selectedEntity != null)
                {
                    if (selectedEntity.Get<Storage>() != null)
                    {
                        //guiManager.OpenStorage(selectedEntity.Get<Storage>());
                    }
                }
            }

            CheckCollision();

            camera.LockToEntity(player);
            camera.LockToMap(TileMap);

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

            TileMap.Draw(GameRef.SpriteBatch, camera);

            entities.Render(GameRef.SpriteBatch);

            camera.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            // GUI Renderer

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        public void AddEntityToTileMap(int x, int y, Entity entity)
        {
            if (TileMap.AddEntity(x, y, entity))
            {
                entity.X = Engine.ToWorldPosX(x);
                entity.Y = Engine.ToWorldPosY(y);
                entities.Add(entity);
            }
        }

        public void CheckCollision()
        {
            Collider collider = player.Get<Collider>();
            Entity entity = player;

            int cellX;
            int cellY;
            Engine.VectorToCell(entity.Position.X, entity.Position.Y, out cellX, out cellY);

            for (int x = cellX - 1; x <= cellX + 1; x++)
            {
                for (int y = cellY - 1; y <= cellY + 1; y++)
                {
                    if (x == cellX && y == cellY)
                        continue;

                    if (!TileMap.GetCollisionLayer().GetTile(x, y).IsWalkable)
                    {
                        float deltaX = (x * Engine.TileWidth + Engine.TileWidth / 2) - (entity.X + collider.OffsetX);
                        float deltaY = (y * Engine.TileHeight + Engine.TileHeight / 2) - (entity.Y + collider.OffsetY);

                        float intersectX = Math.Abs(deltaX) - ((Engine.TileWidth / 2) + (collider.Width / 2));
                        float intersectY = Math.Abs(deltaY) - ((Engine.TileHeight / 2) + (collider.Height / 2));

                        if (intersectX < 0.0f && intersectY < 0.0f)
                        {

                            if (intersectX <= intersectY)
                            {
                                if (deltaY > 0.0f)
                                    entity.Position += new Vector2(0, intersectY);
                                else
                                    entity.Position += new Vector2(0, -intersectY);
                            }
                            else
                            {
                                if (deltaX > 0.0f)
                                    entity.Position += new Vector2(intersectX, 0);
                                else
                                    entity.Position += new Vector2(-intersectX, 0);
                            }
                        }
                    }
                }
            }
        }
    }
}
