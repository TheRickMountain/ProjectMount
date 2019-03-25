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

        public static WorldTimer WorldTimer;

        public static TileSet TileSet;
        public static TileMap TileMap;

        public static EntityList Entities;

        private GUIManager guiManager;

        public static List<Settler> Settlers;

        public static JobList JobSystem;
        public static StockpileList StockpileList;

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Camera = new Camera();

            WorldTimer = new WorldTimer();

            TileSet = new TileSet(ResourceBank.Sprites["tileset"]);
            TileMap = new TileMap(32, 32, TileSet);

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.GetTile(5, 4).BuildingLayerId = TileMap.STONE_1_BLOCK;
            TileMap.GetTile(5, 4).Walkable = false;

            TileMap.GetTile(5, 5).BuildingLayerId = TileMap.STONE_2_BLOCK;
            TileMap.GetTile(5, 5).Walkable = false;

            TileMap.GetTile(5, 3).BuildingLayerId = TileMap.STONE_1_BLOCK;
            TileMap.GetTile(5, 3).Walkable = false;

            TileMap.GetTile(6, 3).BuildingLayerId = TileMap.STONE_2_BLOCK;
            TileMap.GetTile(6, 3).Walkable = false;

            TileMap.GetTile(7, 3).BuildingLayerId = TileMap.STONE_2_BLOCK;
            TileMap.GetTile(7, 3).Walkable = false;

            TileMap.GetTile(7, 5).BuildingLayerId = TileMap.STONE_2_BLOCK;
            TileMap.GetTile(7, 5).Walkable = false;


            TileMap.GetTile(15, 4).GroundLayerId = TileMap.WATER_FRONT_TILE;
            TileMap.GetTile(15, 4).Walkable = false;

            //TileMap.GetTile(15, 5).GroundLayerId = TileMap.WATER_1_TILE;
            //TileMap.GetTile(15, 5).Walkable = false;

            //TileMap.SetTile(15, 4, TileMap.WATER_LEFT_TILE, Layer.GROUND, false);
            //TileMap.SetTile(16, 4, TileMap.WATER_FRONT_TILE, Layer.GROUND, false);
            //TileMap.SetTile(17, 4, TileMap.WATER_FRONT_TILE, Layer.GROUND, false);
            //TileMap.SetTile(18, 4, TileMap.WATER_RIGHT_TILE, Layer.GROUND, false);
            //TileMap.SetTile(15, 5, TileMap.WATER_1_TILE, Layer.GROUND, false);
            //TileMap.SetTile(16, 5, TileMap.WATER_4_TILE, Layer.GROUND, false);
            //TileMap.SetTile(17, 5, TileMap.WATER_2_TILE, Layer.GROUND, false);
            //TileMap.SetTile(18, 5, TileMap.WATER_1_TILE, Layer.GROUND, false);
            //TileMap.SetTile(15, 6, TileMap.WATER_1_TILE, Layer.GROUND, false);
            //TileMap.SetTile(16, 6, TileMap.WATER_1_TILE, Layer.GROUND, false);
            //TileMap.SetTile(17, 6, TileMap.WATER_3_TILE, Layer.GROUND, false);
            //TileMap.SetTile(18, 6, TileMap.WATER_1_TILE, Layer.GROUND, false);

            //AddEntityToTileMap(17, 15, new Campfire());
            TileMap.AddEntity(5, 20, new Tree(), false);

            TileMap.GetTile(10, 17).AddItem(ItemDatabase.GetItemById(TileMap.FLINT), 1);
            TileMap.GetTile(10, 18).AddItem(ItemDatabase.GetItemById(TileMap.FLINT), 1);
            TileMap.GetTile(10, 19).AddItem(ItemDatabase.GetItemById(TileMap.FLINT), 1);
            TileMap.GetTile(10, 20).AddItem(ItemDatabase.GetItemById(TileMap.FLINT), 1);

            TileMap.GetTile(11, 17).AddItem(ItemDatabase.GetItemById(TileMap.STICK), 1);
            TileMap.GetTile(11, 18).AddItem(ItemDatabase.GetItemById(TileMap.STICK), 1);
            TileMap.GetTile(11, 19).AddItem(ItemDatabase.GetItemById(TileMap.STICK), 1);
            TileMap.GetTile(11, 20).AddItem(ItemDatabase.GetItemById(TileMap.STICK), 1);

            TileMap.AddEntity(12, 17, new Grass());
            TileMap.AddEntity(12, 18, new Grass());
            TileMap.AddEntity(12, 19, new Grass());
            TileMap.AddEntity(12, 20, new Grass());

            TileMap.AddEntity(9, 17, new Bush());
            TileMap.AddEntity(9, 18, new Bush());
            TileMap.AddEntity(9, 19, new Bush());
            TileMap.AddEntity(9, 20, new Bush());

            Settlers = new List<Settler>();

            Settler settler = new Settler(Engine.ToWorldPos(18), Engine.ToWorldPos(15));
            Entities.Add(settler);
            Settlers.Add(settler);

            settler = new Settler(Engine.ToWorldPos(14), Engine.ToWorldPos(15));
            Entities.Add(settler);
            Settlers.Add(settler);

            settler = new Settler(Engine.ToWorldPos(16), Engine.ToWorldPos(18));
            Entities.Add(settler);
            Settlers.Add(settler);

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

            WorldTimer.Update(gameTime);

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