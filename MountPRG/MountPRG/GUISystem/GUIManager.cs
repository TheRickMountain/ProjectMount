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
        public static int SLOT_SIZE = 48;
        public static int BUTTON_SIZE = 48;
        public static int OFFSET = 5;

        private List<IGUI> guiElements = new List<IGUI>();

        private ItemDatabase itemDatabase;

        public static DayNightSystemGUI DayNightSystemGUI;
        public static ActionPanelGUI ActionPanelGUI;
        public static StockpileGUI StockpileGUI;
        public static HutUI HutUI;

        public static bool MouseOnUI { get; set; }

        public GUIManager(Game game)
        {
            itemDatabase = new ItemDatabase();

            DayNightSystemGUI = new DayNightSystemGUI(true);
            ActionPanelGUI = new ActionPanelGUI(true);
            StockpileGUI = new StockpileGUI(false);
            HutUI = new HutUI(false);

            guiElements.Add(DayNightSystemGUI);
            guiElements.Add(ActionPanelGUI);
            guiElements.Add(StockpileGUI);
            guiElements.Add(HutUI);
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
