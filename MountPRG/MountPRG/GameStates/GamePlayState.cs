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

        public static List<Entity> Characters;

        public static JobList JobSystem;
        public static StockpileList StockpileList;

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Camera = new Camera();

            TileMap = new TileMap(32, 32, new TileSet(ResourceBank.Sprites["tileset"]));

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.SetTile(5, 4, TileMap.STONE_BLOCK_1, Layer.BUILDING, false);
            TileMap.SetTile(5, 5, TileMap.STONE_BLOCK_2, Layer.BUILDING, false);
            TileMap.SetTile(5, 3, TileMap.STONE_BLOCK_1, Layer.BUILDING, false);
            TileMap.SetTile(6, 3, TileMap.STONE_BLOCK_2, Layer.BUILDING, false);
            TileMap.SetTile(7, 3, TileMap.STONE_BLOCK_2, Layer.BUILDING, false);
            TileMap.SetTile(7, 5, TileMap.STONE_BLOCK_2, Layer.BUILDING, false);

            //AddEntityToTileMap(17, 15, new Campfire());
            TileMap.AddEntity(10, 15, new Tree(), false);
            TileMap.AddEntity(10, 18, new Flint());
            TileMap.AddEntity(10, 19, new Flint());
            TileMap.AddEntity(10, 20, new Flint());
            TileMap.AddEntity(10, 21, new Flint());
            TileMap.AddEntity(19, 15, new Stick());
            TileMap.AddEntity(20, 15, new Stick());
            TileMap.AddEntity(21, 15, new Stick());
            TileMap.AddEntity(22, 15, new Stick());
            TileMap.AddEntity(24, 15, new Bush());
            TileMap.AddEntity(15, 16, new Grass());
            TileMap.AddEntity(16, 16, new Grass());
            TileMap.AddEntity(16, 17, new Grass());
            TileMap.AddEntity(15, 17, new Grass());

            Entities.Add(new Settler(Engine.ToWorldPos(18), Engine.ToWorldPos(15)));
            Entities.Add(new Settler(Engine.ToWorldPos(14), Engine.ToWorldPos(15)));
            Entities.Add(new Settler(Engine.ToWorldPos(16), Engine.ToWorldPos(18)));

            JobSystem = new JobList();
            StockpileList = new StockpileList();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            guiManager.Update(gameTime);

            Camera.Update(gameTime);

            Entities.UpdateList();

            Entities.Update(gameTime);

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

            Entities.Render(GameRef.SpriteBatch);

            Camera.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            // GUI Renderer

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }
    }
}