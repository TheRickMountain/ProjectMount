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


        public static TileSet TileSet;
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

            TileSet = new TileSet(ResourceBank.Sprites["tileset"]);
            TileMap = new TileMap(32, 32, TileSet);

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.SetTile(5, 4, TileMap.STONE_1_BLOCK, Layer.BUILDING, false);
            TileMap.SetTile(5, 5, TileMap.STONE_2_BLOCK, Layer.BUILDING, false);
            TileMap.SetTile(5, 3, TileMap.STONE_1_BLOCK, Layer.BUILDING, false);
            TileMap.SetTile(6, 3, TileMap.STONE_2_BLOCK, Layer.BUILDING, false);
            TileMap.SetTile(7, 3, TileMap.STONE_2_BLOCK, Layer.BUILDING, false);
            TileMap.SetTile(7, 5, TileMap.STONE_2_BLOCK, Layer.BUILDING, false);

            //AddEntityToTileMap(17, 15, new Campfire());
            TileMap.AddEntity(10, 15, new Tree(), false);

            TileMap.AddEntity(10, 17, new Flint());
            TileMap.AddEntity(10, 18, new Flint());
            TileMap.AddEntity(10, 19, new Flint());
            TileMap.AddEntity(10, 20, new Flint());

            TileMap.AddEntity(11, 17, new Stick());
            TileMap.AddEntity(11, 18, new Stick());
            TileMap.AddEntity(11, 19, new Stick());
            TileMap.AddEntity(11, 20, new Stick());

            TileMap.AddEntity(12, 17, new Grass());
            TileMap.AddEntity(12, 18, new Grass());
            TileMap.AddEntity(12, 19, new Grass());
            TileMap.AddEntity(12, 20, new Grass());


            TileMap.AddEntity(9, 17, new Bush());
            TileMap.AddEntity(9, 18, new Bush());
            TileMap.AddEntity(9, 19, new Bush());
            TileMap.AddEntity(9, 20, new Bush());

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