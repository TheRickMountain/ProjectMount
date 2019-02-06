using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using MountPRG.StateManager;

namespace MountPRG.GameStates
{
    public class BaseGameState : GameState
    {
        #region Field Region

        protected static Random random = new Random();

        protected Game1 GameRef;

        #endregion

        #region Contructor Region

        public BaseGameState(Game game) : base(game)
        {
            GameRef = (Game1)game;
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
