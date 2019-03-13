using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class GUIManager
    {
        public static int SLOT_SIZE = 56;
        public static int BUTTON_SIZE = 48;
        public static int OFFSET = 5;

        private List<IGUI> guiElements = new List<IGUI>();

        private ItemDatabase itemDatabase;

        private DayNightSystemGUI dayNightSystemGUI;
        public static ActionPanelGUI ActionPanelGUI;

        public static bool MouseOnUI { get; set; }

        public GUIManager(Game game)
        {
            itemDatabase = new ItemDatabase();

            dayNightSystemGUI = new DayNightSystemGUI(true);
            ActionPanelGUI = new ActionPanelGUI(true);

            guiElements.Add(dayNightSystemGUI);
            guiElements.Add(ActionPanelGUI);
        }

        public void Update(GameTime gameTime)
        {
            MouseOnUI = false;

            for (int i = 0; i < guiElements.Count; i++)
            {
                if (guiElements[i].Active)
                    guiElements[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < guiElements.Count; i++)
            {
                if (guiElements[i].Active)
                    guiElements[i].Draw(spriteBatch);
            }
        }
    }
}
