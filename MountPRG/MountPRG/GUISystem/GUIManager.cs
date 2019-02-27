using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class GUIManager : IGUI
    {
        private List<IGUI> guiElements = new List<IGUI>();

        private TimeSystemGUI worldTime;

        public GUIManager(Game game)
        {
            worldTime = new TimeSystemGUI(game);

            guiElements.Add(worldTime);
        }

        public void Update(GameTime gameTime)
        {
            foreach(IGUI e in guiElements)
                e.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IGUI e in guiElements)
                e.Draw(spriteBatch);
        }

    }
}
