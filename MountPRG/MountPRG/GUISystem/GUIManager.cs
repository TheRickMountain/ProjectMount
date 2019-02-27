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

        private DayNightSystemGUI worldTime;
        private ActiveInventory activeInventory;
        private static StorageGUI storageGUI;

        public GUIManager(Game game)
        {
            worldTime = new DayNightSystemGUI(true);
            activeInventory = new ActiveInventory(6, true);
            storageGUI = new StorageGUI(false);

            guiElements.Add(worldTime);
            guiElements.Add(activeInventory);
            guiElements.Add(storageGUI);
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

        public static void OpenStorage(Storage storage)
        {
            storageGUI.Open(storage);
        }

        public static void CloseStoarge()
        {
            storageGUI.Close();
        }

    }
}
