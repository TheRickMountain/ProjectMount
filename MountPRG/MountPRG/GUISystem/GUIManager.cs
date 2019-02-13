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

        private TimeSystem worldTime;

        private Slot selectedSettler;

        public GUIManager(Game game)
        {
            worldTime = new TimeSystem(game);

            guiElements.Add(worldTime);

            selectedSettler = new Slot(GamePlayState.SlotTexture, 48, 48);
            selectedSettler.PositionX = Game1.ScreenRectangle.Width - selectedSettler.Width - 5;
            selectedSettler.PositionY = Game1.ScreenRectangle.Height - selectedSettler.Height - 5;
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

            if (selectedSettler.HasItem)
                selectedSettler.Draw(spriteBatch);
        }

        public void SetSelectedSettler(Avatar avatar)
        {
            if (avatar == null)
            {
                selectedSettler.RemoveItem();
            }
            else
            {
                selectedSettler.AddItem(avatar.Texture);
            }
        }

    }
}
