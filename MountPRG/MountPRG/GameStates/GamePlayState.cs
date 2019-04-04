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

        public static ItemDatabase ItemDatabase;
        public static NewTileset NewTileset;
        public static TileSet TileSet;
        public static TileMap TileMap;

        public static EntityList Entities;

        private GUIManager guiManager;

        public static List<Settler> Settlers;

        public static JobList JobList;
        public static List<Stockpile> Stockpiles;
        public static List<Farm> Farms;

        public static WorldManager WorldManager;

        private Effect effect;

        public Texture2D lightMask;
        public Effect lighting;
        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            NewTileset = new NewTileset(ResourceBank.Sprites["tileset"], TileMap.TILE_SIZE, TileMap.TILE_SIZE);
            ItemDatabase = new ItemDatabase();
            Settlers = new List<Settler>();
            Camera = new Camera();
            WorldManager = new WorldManager(Camera);

            WorldTimer = new WorldTimer();
            WorldTimer.SetTime(0);
            
            TileSet = new TileSet(ResourceBank.Sprites["tileset"]);
            TileMap = new TileMap(32, 32, NewTileset);

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

            TileMap.GetTile(7, 3).BuildingLayerId = TileMap.STONE_1_BLOCK;
            TileMap.GetTile(7, 3).Walkable = false;

            TileMap.GetTile(7, 4).BuildingLayerId = TileMap.STONE_1_BLOCK;
            TileMap.GetTile(7, 4).Walkable = false;

            TileMap.GetTile(7, 5).BuildingLayerId = TileMap.STONE_2_BLOCK;
            TileMap.GetTile(7, 5).Walkable = false;


            TileMap.GetTile(15, 4).GroundLayerId = TileMap.WATER_LEFT_TILE;
            TileMap.GetTile(15, 4).Walkable = false;

            TileMap.GetTile(16, 4).GroundLayerId = TileMap.WATER_FRONT_TILE;
            TileMap.GetTile(16, 4).Walkable = false;

            TileMap.GetTile(17, 4).GroundLayerId = TileMap.WATER_FRONT_TILE;
            TileMap.GetTile(17, 4).Walkable = false;

            TileMap.GetTile(18, 4).GroundLayerId = TileMap.WATER_RIGHT_TILE;
            TileMap.GetTile(18, 4).Walkable = false;

            TileMap.GetTile(15, 5).GroundLayerId = TileMap.WATER_1_TILE;
            TileMap.GetTile(15, 5).Walkable = false;

            TileMap.GetTile(16, 5).GroundLayerId = TileMap.WATER_4_TILE;
            TileMap.GetTile(16, 5).Walkable = false;

            TileMap.GetTile(17, 5).GroundLayerId = TileMap.WATER_2_TILE;
            TileMap.GetTile(17, 5).Walkable = false;

            TileMap.GetTile(18, 5).GroundLayerId = TileMap.WATER_1_TILE;
            TileMap.GetTile(18, 5).Walkable = false;

            TileMap.GetTile(15, 6).GroundLayerId = TileMap.WATER_1_TILE;
            TileMap.GetTile(15, 6).Walkable = false;

            TileMap.GetTile(16, 6).GroundLayerId = TileMap.WATER_1_TILE;
            TileMap.GetTile(16, 6).Walkable = false;

            TileMap.GetTile(17, 6).GroundLayerId = TileMap.WATER_3_TILE;
            TileMap.GetTile(17, 6).Walkable = false;

            TileMap.GetTile(18, 6).GroundLayerId = TileMap.WATER_1_TILE;
            TileMap.GetTile(18, 6).Walkable = false;

            TileMap.AddEntity(16, 15, new Campfire(), false);
            TileMap.AddEntity(6, 4, new Tree(), false);

            TileMap.AddEntity(6, 9, new Tree(), false);

            TileMap.GetTile(10, 17).AddItem(ItemDatabase[Item.FLINT], 1);
            TileMap.GetTile(10, 18).AddItem(ItemDatabase[Item.FLINT], 1);
            TileMap.GetTile(10, 19).AddItem(ItemDatabase[Item.FLINT], 1);
            TileMap.GetTile(10, 20).AddItem(ItemDatabase[Item.FLINT], 1);

            TileMap.GetTile(11, 17).AddItem(ItemDatabase[Item.STICK], 1);
            TileMap.GetTile(11, 18).AddItem(ItemDatabase[Item.STICK], 1);
            TileMap.GetTile(11, 19).AddItem(ItemDatabase[Item.STICK], 1);
            TileMap.GetTile(11, 20).AddItem(ItemDatabase[Item.STICK], 1);

            TileMap.AddEntity(12, 17, new Grass());
            TileMap.AddEntity(12, 18, new Grass());
            TileMap.AddEntity(12, 19, new Grass());
            TileMap.AddEntity(12, 20, new Grass());

            TileMap.AddEntity(9, 17, new Bush());
            TileMap.AddEntity(9, 18, new Bush());
            TileMap.AddEntity(9, 19, new Bush());
            TileMap.AddEntity(9, 20, new Bush());

            TileMap.AddEntity(14, 20, new Wheat());
            TileMap.AddEntity(15, 20, new Wheat());
            TileMap.AddEntity(14, 21, new Wheat());
            TileMap.AddEntity(15, 21, new Wheat());

            TileMap.AddEntity(14, 22, new Wheat());
            TileMap.AddEntity(15, 22, new Wheat());
            TileMap.AddEntity(14, 23, new Wheat());
            TileMap.AddEntity(15, 23, new Wheat());

            TileMap.AddEntity(16, 20, new Barley());
            TileMap.AddEntity(17, 20, new Barley());
            TileMap.AddEntity(16, 21, new Barley());
            TileMap.AddEntity(17, 21, new Barley());

            TileMap.AddEntity(16, 22, new Barley());
            TileMap.AddEntity(17, 22, new Barley());
            TileMap.AddEntity(16, 23, new Barley());
            TileMap.AddEntity(17, 23, new Barley());


            Settler settler = new Settler(Engine.ToWorldPos(18), Engine.ToWorldPos(15));
            Entities.Add(settler);
            Settlers.Add(settler);

            settler = new Settler(Engine.ToWorldPos(14), Engine.ToWorldPos(15));
            Entities.Add(settler);
            Settlers.Add(settler);

            settler = new Settler(Engine.ToWorldPos(16), Engine.ToWorldPos(18));
            Entities.Add(settler);
            Settlers.Add(settler);

            JobList = new JobList();
            Stockpiles = new List<Stockpile>();
            Farms = new List<Farm>();

            effect = ResourceBank.Effects["File"];

            lightMask = ResourceBank.Sprites["lightmask"];
            lighting = ResourceBank.Effects["Lighting"];
            var pp = GraphicsDevice.PresentationParameters;
            lightsTarget = new RenderTarget2D(
                GraphicsDevice, Game1.ScreenRectangle.Width, Game1.ScreenRectangle.Height);
            mainTarget = new RenderTarget2D(
                GraphicsDevice, Game1.ScreenRectangle.Width, Game1.ScreenRectangle.Height);

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
            WorldManager.Update(gameTime);

            WorldTimer.Update(gameTime);

            Entities.UpdateList();

            Entities.Update(gameTime);

            JobList.Update();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(lightsTarget);
            GraphicsDevice.Clear(Color.Transparent);
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, Camera.Transformation);
            GameRef.SpriteBatch.Draw(lightMask, new Vector2((16 * TileMap.TILE_SIZE + 8) - lightMask.Width / 2, 
                (15 * TileMap.TILE_SIZE + 8) - lightMask.Height / 2), Color.White);
            GameRef.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(mainTarget);
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, Camera.Transformation);
            effect.Parameters["ambientColor"].SetValue(Color.White.ToVector4());

            effect.CurrentTechnique.Passes[0].Apply();

            TileMap.Draw(GameRef.SpriteBatch, Camera);

            Entities.Render(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend);
            lighting.Parameters["lightMask"].SetValue(lightsTarget);
            lighting.Parameters["ambient"].SetValue(GUIManager.DayNightSystemUI.CurrentColor.ToVector4());
            lighting.Parameters["dayColor"].SetValue(GUIManager.DayNightSystemUI.DayColor.ToVector4());
            lighting.CurrentTechnique.Passes[0].Apply();
            GameRef.SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            //effect.Parameters["ambientColor"].SetValue(Color.White.ToVector4());

            //effect.CurrentTechnique.Passes[0].Apply();

            //WorldManager.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();

            // GUI Renderer
            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }
    }
}