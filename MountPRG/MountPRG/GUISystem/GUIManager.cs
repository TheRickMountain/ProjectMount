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
        private List<IGUI> guiElements = new List<IGUI>();

        private ItemDatabase itemDatabase;

        private DayNightSystemGUI dayNightSystemGUI;
        public static ActiveInventoryGUI ActiveInventoryGUI;
        public static StorageGUI StorageGUI;
        

        public GUIManager(Game game)
        {
            itemDatabase = new ItemDatabase();

            dayNightSystemGUI = new DayNightSystemGUI(true);
            ActiveInventoryGUI = new ActiveInventoryGUI(6, true);
            StorageGUI = new StorageGUI(false);

            guiElements.Add(dayNightSystemGUI);
            guiElements.Add(ActiveInventoryGUI);
            guiElements.Add(StorageGUI);
        }

        public void Update(GameTime gameTime)
        {
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
