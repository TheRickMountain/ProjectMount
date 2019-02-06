using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MountPRG.Entities;

namespace MountPRG.GUISystem
{
    public class GUIManager : IGUI
    {

        private List<IGUI> guiElements = new List<IGUI>();

        private List<Slot> slots = new List<Slot>();

        private ProgressBar hitPoints;
        private ProgressBar hungerPoints;

        private TimeSystem worldTime;

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

            hitPoints = new ProgressBar(game, Color.Red);
            hitPoints.X = offset;
            hitPoints.Y = 100;
            hitPoints.Width = 120;
            hitPoints.Height = 15;

            hungerPoints = new ProgressBar(game, Color.Orange);
            hungerPoints.X = offset;
            hungerPoints.Y = hitPoints.Y + hitPoints.Height + offset;
            hungerPoints.Width = 120;
            hungerPoints.Height = 15;

            guiElements.Add(hitPoints);

            guiElements.Add(hungerPoints);

            worldTime = new TimeSystem(game);

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

        public void AddItem(Entity entity)
        {
            slots[0].AddItem(entity);
        }

    }
}
