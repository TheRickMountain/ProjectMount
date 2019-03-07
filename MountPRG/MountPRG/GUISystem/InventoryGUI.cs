using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class InventoryGUI : IGUI
    {
        private UI inventoryBackground;
        private UI equipmentBackground;

        private List<Slot> slots;

        private Slot headSlot;
        private Slot bodySlot;
        private Slot legsSlot;
        private Slot weaponOne;
        private Slot weaponTwo;

        public InventoryGUI(bool active) : base(active)
        {
            inventoryBackground = new UI();
            equipmentBackground = new UI();

            slots = new List<Slot>();

            int columns = 6;
            int rows = 3;

            inventoryBackground.InnerWidth = GUIManager.SLOT_SIZE * columns + GUIManager.OFFSET * (columns - 1);
            inventoryBackground.InnerHeight = GUIManager.SLOT_SIZE * rows + GUIManager.OFFSET * (rows - 1);
            inventoryBackground.X = Game1.ScreenRectangle.Width / 2 - inventoryBackground.Width / 2;
            inventoryBackground.Y = Game1.ScreenRectangle.Height / 2;

            equipmentBackground.InnerWidth = inventoryBackground.InnerWidth;
            equipmentBackground.InnerHeight = inventoryBackground.InnerHeight;
            equipmentBackground.X = inventoryBackground.X;
            equipmentBackground.Y = inventoryBackground.Y - equipmentBackground.Height - GUIManager.OFFSET;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Slot slot = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
                    slot.X = inventoryBackground.InnerX + x * GUIManager.SLOT_SIZE + x * GUIManager.OFFSET;
                    slot.Y = inventoryBackground.InnerY + y * GUIManager.SLOT_SIZE + y * GUIManager.OFFSET;
                    slots.Add(slot);
                }
            }

            slots[0].AddItem(ItemDatabase.GetItemById(ItemDatabase.BERRY), 1);
            slots[1].AddItem(ItemDatabase.GetItemById(ItemDatabase.AXE), 1);

            headSlot = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
            headSlot.AddItem(ItemDatabase.GetItemById(ItemDatabase.HEAD), 1);

            bodySlot = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
            bodySlot.AddItem(ItemDatabase.GetItemById(ItemDatabase.BODY), 1);

            legsSlot = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
            legsSlot.AddItem(ItemDatabase.GetItemById(ItemDatabase.LEGS), 1);

            weaponOne = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
            weaponOne.AddItem(ItemDatabase.GetItemById(ItemDatabase.WEAPON), 1);

            weaponTwo = new Slot(ResourceBank.SlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
            weaponTwo.AddItem(ItemDatabase.GetItemById(ItemDatabase.WEAPON), 1);

            headSlot.X = equipmentBackground.InnerX + GUIManager.SLOT_SIZE + GUIManager.OFFSET;
            headSlot.Y = equipmentBackground.InnerY;

            weaponOne.X = equipmentBackground.InnerX;
            weaponOne.Y = equipmentBackground.InnerY + GUIManager.SLOT_SIZE + GUIManager.OFFSET;

            bodySlot.X = weaponOne.X + GUIManager.SLOT_SIZE + GUIManager.OFFSET;
            bodySlot.Y = weaponOne.Y;

            weaponTwo.X = bodySlot.X + GUIManager.SLOT_SIZE + GUIManager.OFFSET;
            weaponTwo.Y = bodySlot.Y;

            legsSlot.X = bodySlot.X;
            legsSlot.Y = bodySlot.Y + GUIManager.SLOT_SIZE + GUIManager.OFFSET;
        }
        
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            equipmentBackground.Draw(spriteBatch);
            headSlot.Draw(spriteBatch);
            weaponOne.Draw(spriteBatch);
            bodySlot.Draw(spriteBatch);
            weaponTwo.Draw(spriteBatch);
            legsSlot.Draw(spriteBatch);

            inventoryBackground.Draw(spriteBatch);
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }
        }
    }
}
