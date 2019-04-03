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
        public static int CHECKBOX_SIZE = 16;
        public static int OFFSET = 5;

        private List<UI> guiElements = new List<UI>();

        private ItemDatabase itemDatabase;

        public static TimeSystemUI DayNightSystemUI;
        public static ActionPanelUI ActionPanelUI;
        public static StockpileUI StockpileUI;
        public static HutUI HutUI;
        public static FarmUI FarmUI;
        public static BuildUI BuildUI;

        public static bool MouseOnUI { get; set; }

        public GUIManager(Game game)
        {
            itemDatabase = new ItemDatabase();

            DayNightSystemUI = new TimeSystemUI();
            guiElements.Add(DayNightSystemUI);

            ActionPanelUI = new ActionPanelUI();
            ActionPanelUI.Enable();
            guiElements.Add(ActionPanelUI);

            StockpileUI = new StockpileUI();
            StockpileUI.Disable();
            guiElements.Add(StockpileUI);

            HutUI = new HutUI();
            FarmUI = new FarmUI();
            BuildUI = new BuildUI();
        }

        public void Update(GameTime gameTime)
        {
            MouseOnUI = false;

            if(InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                StockpileUI.Close();
                HutUI.Close();
                FarmUI.Close();
            }

            if (HutUI.Active)
                HutUI.Update(gameTime);
            else if (FarmUI.Active)
                FarmUI.Update(gameTime);
            else if (BuildUI.Active)
                BuildUI.Update(gameTime);

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

            if (HutUI.Active)
                HutUI.Draw(spriteBatch);
            else if (FarmUI.Active)
                FarmUI.Draw(spriteBatch);
            else if (BuildUI.Active)
                BuildUI.Draw(spriteBatch);
        }
    }
}
