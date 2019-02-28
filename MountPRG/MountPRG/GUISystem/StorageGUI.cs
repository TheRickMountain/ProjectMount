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
            
            for (int y = 0, i = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++, i++)
                {
                    Slot slot = new Slot(ResourceBank.SlotTexture, 50, 50, true);
                    slot.X = xStart + x * 50 + x * 5;
                    slot.Y = yStart + y * 50 + y * 5;

                    if (storage.Items[i] != -1)
                    {
                        slot.AddItem(ItemDatabase.GetItemById(storage.Items[i]), storage.Count[i]);
                    }

                    slots.Add(slot);
                }
            }
        }

        public void Close()
        {
            for(int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.HasItem)
                {
                    storage.Items[i] = slot.Item.Id;
                    storage.Count[i] = slot.Count;
                }
                else
                {
                    storage.Items[i] = -1;
                    storage.Count[i] = 0;
                }

            }

            Active = false;
            slots.Clear();
        }

        public int AddItem(Item itemToAdd, int count)
        {
            if (Active)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].HasItem)
                    {
                        if (slots[i].Item == itemToAdd && itemToAdd.Stackable && slots[i].Count < 99)
                        {
                            // Вычисляем общее количество добавляемых и предметов в слоту
                            int itemsCount = slots[i].Count + count;

                            // Если общее количество предметов больше 99
                            if (itemsCount > 99)
                            {
                                // Добавляем в слот 99 предметов, а count присваиваем оставшееся количество предметов
                                slots[i].AddItem(itemToAdd, 99);
                                count = itemsCount - 99;
                            }
                            else
                            {
                                // Иначе просто добавляем предметы
                                slots[i].AddItem(itemToAdd, itemsCount);
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        if (count > 99)
                        {
                            slots[i].AddItem(itemToAdd, 99);
                            count -= 99;
                        }
                        else
                        {
                            slots[i].AddItem(itemToAdd, count);
                            return 0;
                        }
                    }
                }
                return count;
            }

            return -1;
        }

        public Slot getSelectedSlot()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Intersects(InputManager.GetX(), InputManager.GetY()))
                {
                    return slots[i];
                }
            }
            return null;
        }

    }
}
