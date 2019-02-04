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

        public GamePlayState(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            engine = Engine.GetInstance(16, 16);
            camera = new Camera();

            TileSet tileSet = new TileSet(256 / 16, 256 / 16, Engine.TileWidth, Engine.TileHeight);
            tileSet.Texture = content.Load<Texture2D>(@"tileset");

            TileLayer groundLayer = new TileLayer(50, 50, 0);
            groundLayer.SetTile(7, 7, 1);
            groundLayer.SetTile(8, 7, 1);

            TileLayer edgeLayer = new TileLayer(50, 50, -1);
            edgeLayer.Visible = false;

            TileLayer buildingLayer = new TileLayer(50, 50, -1);
            buildingLayer.Visible = false;

            tileMap = new TileMap(tileSet, groundLayer, edgeLayer, buildingLayer);

            //tileMap.CollisionLayer.SetCollider(7, 7, CollisionType.Impassable);

            camera.ToggleCameraMode();

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

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

    }
}
