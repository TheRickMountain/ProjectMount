using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class StorageGUI : IGUI
    {

        private List<Slot> slots;
        private Storage storage;

        public StorageGUI(bool active) : base(active)
        {
            slots = new List<Slot>();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }
        }

        public void Open(Storage storage)
        {
            this.storage = storage;

            Active = true;

            int[] items = storage.Items;
            int[] count = storage.Count;
            int rows = storage.Rows;
            int columns = storage.Columns;

            int xStart = (Game1.ScreenRectangle.Width / 2) - ((columns * 50 + (columns - 1) * 5) / 2);
            int yStart = (Game1.ScreenRectangle.Height / 2) - ((rows * 50 + (rows - 1) * 5) / 2);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Slot slot = new Slot(TextureBank.SlotTexture, 50, 50, true);
                    slot.X = xStart + x * 50 + x * 5;
                    slot.Y = yStart + y * 50 + y * 5;
                    slots.Add(slot);
                }
            }
        }

        public void Close()
        {
            Active = false;
            slots.Clear();
        }
    }
}
