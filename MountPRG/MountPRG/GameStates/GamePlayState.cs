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
        public static Texture2D SettlerTexture;
        public static Texture2D TilesetTexture;
        public static Texture2D SelectorTexture;

        private bool paused;
        private Camera camera;
        private TileMap tileMap;

        private GUIManager guiManager;

        private EntityList entities;

        public GamePlayState(Game game) : base(game)
        {
            entities = new EntityList();
        }

        public override void Initialize()
        {
            SettlerTexture = content.Load<Texture2D>(@"human");
            TilesetTexture = content.Load<Texture2D>(@"tileset");
            SelectorTexture = content.Load<Texture2D>(@"selector");

            Engine engine = Engine.GetInstance(16, 16);
            camera = new Camera();

            tileMap = new TileMap(
                new TileSet(TilesetTexture, Engine.TileWidth, Engine.TileHeight), 50, 50);

            tileMap.SetEdgeLayer(5, 4, TileMap.STONE_BLOCK_1, CollisionType.Impassable);
            tileMap.SetEdgeLayer(5, 5, TileMap.STONE_BLOCK_2, CollisionType.Impassable);
            tileMap.SetEdgeLayer(5, 3, TileMap.STONE_BLOCK_1, CollisionType.Impassable);
            tileMap.SetEdgeLayer(6, 3, TileMap.STONE_BLOCK_2, CollisionType.Impassable);

            Settler settler = new Settler(new Vector2(Engine.ToWorldPosX(1), Engine.ToWorldPosY(5)));
            entities.Add(settler);

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
            camera.LockToMap(tileMap);

            entities.UpdateList();

            if(!paused)
                entities.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

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

            entities.Render(GameRef.SpriteBatch);

            camera.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            // GUI Renderer

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            guiManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }
    }
}
