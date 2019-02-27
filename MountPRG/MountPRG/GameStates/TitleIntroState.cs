using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public interface ITitleIntroState : IGameState
    {

    }

    public class TitleIntroState : BaseGameState, ITitleIntroState
    {

        public TitleIntroState(Game game) : base(game)
        {

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
            if(InputManager.GetKey(Keys.Space))
            {
                manager.ChangeState((MainMenuState)GameRef.MainMenuState, PlayerIndex.One);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
