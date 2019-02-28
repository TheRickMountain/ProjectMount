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

        private Player player;

        public static List<Entity> Characters;

        private Effect effect;

        public GamePlayState(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            Camera = new Camera();

            TileMap= new TileMap(50, 50, new TileSet(ResourceBank.TilesetTexture));

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.SetTile(5, 4, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(5, 5, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(5, 3, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(6, 3, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);

            AddEntityToTileMap(15, 10, new Chest());
            AddEntityToTileMap(16, 10, new Chest());
            AddEntityToTileMap(10, 15, new Tree());
            AddEntityToTileMap(15, 16, new Wood());
            AddEntityToTileMap(16, 16, new Wood());
            AddEntityToTileMap(17, 16, new Wood());
            AddEntityToTileMap(18, 16, new Wood());
            AddEntityToTileMap(15, 17, new Stone());
            AddEntityToTileMap(16, 17, new Stone());

            player = new Player(Engine.ToWorldPos(15), Engine.ToWorldPos(15));
            Entities.Add(player);
            Wolf wolf = new Wolf(Engine.ToWorldPos(18), Engine.ToWorldPos(15));
            Entities.Add(wolf);

            Characters = new List<Entity>();
            Characters.Add(player);

            effect = content.Load<Effect>(@"Test");

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

            Camera.LockToEntity(player);
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

            effect.CurrentTechnique.Passes[0].Apply();
            //effect.Parameters["lightPosition"].SetValue(new Vector2(16*10, 16*10));

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

        public void AddEntityToTileMap(int x, int y, Entity entity)
        {
            if (TileMap.AddEntity(x, y, entity, entity.Walkable))
            {
                entity.X = Engine.ToWorldPos(x);
                entity.Y = Engine.ToWorldPos(y);
                Entities.Add(entity);
            }
        }
    }
}