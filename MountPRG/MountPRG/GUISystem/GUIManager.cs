using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.GUISystem
{
    public class GUIManager : IGUI
    {

        private List<IGUI> guiElements = new List<IGUI>();

        private List<Slot> slots = new List<Slot>();

        public GUIManager(Game game)
        {
            int offset = 5;

            Texture2D slotTexture = game.Content.Load<Texture2D>(@"slot");

            int allSlotsWidth = 5 * 48 + 4 * offset;

            for (int i = 0; i < 5; i++)
            {
                Slot slot = new Slot(slotTexture, 48, 48);
                slot.PositionX = Game1.ScreenRectangle.Width / 2 - allSlotsWidth / 2 + i * (slot.Width + offset);
                slot.PositionY = Game1.ScreenRectangle.Height - slot.Height - offset;
                slots.Add(slot);
            }
        
            guiElements.AddRange(slots);
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
