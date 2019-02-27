using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public interface IMainMenuState : IGameState
    {
    }

    public class MainMenuState : BaseGameState, IMainMenuState
    {
        public MainMenuState(Game game) : base(game)
        {
            game.Services.AddService(typeof(IMainMenuState), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyDown(Keys.Space))
            {
                manager.ChangeState((GamePlayState)GameRef.GamePlayState, PlayerIndex.One);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
