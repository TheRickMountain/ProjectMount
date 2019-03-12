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

        public static JobQueue JobSystem;

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Camera = new Camera();

            TileMap = new TileMap(32, 32, new TileSet(ResourceBank.Sprites["tileset"]));

            guiManager = new GUIManager(GameRef);

            Entities = new EntityList();

            TileMap.SetTile(5, 4, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(5, 5, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(5, 3, TileMap.STONE_BLOCK_1, Layer.ENTITY, false);
            TileMap.SetTile(6, 3, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(7, 3, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);
            TileMap.SetTile(7, 5, TileMap.STONE_BLOCK_2, Layer.ENTITY, false);

            //AddEntityToTileMap(17, 15, new Campfire());
            AddEntityToTileMap(10, 15, new Tree());
            AddEntityToTileMap(10, 18, new Flint());
            AddEntityToTileMap(19, 15, new Stick());

            Player = new Player(Engine.ToWorldPos(15), Engine.ToWorldPos(15));
            Entities.Add(Player);

            Characters = new List<Entity>();
            Characters.Add(Player);

            JobSystem = new JobQueue();

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

        public void AddEntityToTileMap(int x, int y, Entity entity)
        {
            if (TileMap.AddEntity(x, y, entity, entity.Walkable))
            {
                entity.X = Engine.ToWorldPos(x);
                entity.Y = Engine.ToWorldPos(y);
                Entities.Add(entity);
            }
        }

        public void Generate(string name, int count)
        {
            for(int i = 0; i < count; i++)
            {
                Entity entity = null;
                if(name.Equals("tree"))
                {
                    entity = new Tree();
                }
                else if(name.Equals("bush"))
                {
                    entity = new Bush();
                }
                else if(name.Equals("flint"))
                {
                    entity = new Flint();
                }
                else if(name.Equals("stick"))
                {
                    entity = new Stick();
                }

                int x = MyRandom.Range(0, TileMap.Width - 1);
                int y = MyRandom.Range(0, TileMap.Height - 1);
                AddEntityToTileMap(x, y, entity);
            }
        }
    }
}