﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class ActiveInventoryGUI : IGUI
    {

        private List<Slot> slots;

        public ActiveInventoryGUI(int count, bool active) : base(active)
        {
            slots = new List<Slot>();
            int xStart = (Game1.ScreenRectangle.Width / 2) - ((count * 50 + (count - 1) * 5) / 2);
            int yStart = Game1.ScreenRectangle.Height - 55;
            for (int i = 0; i < count; i++)
            {
                Slot slot = new Slot(ResourceBank.SlotTexture, 50, 50, true);
                slot.X = xStart + i * 50 + i * 5;
                slot.Y = yStart;
                slots.Add(slot);
            }

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }
        }

        public int AddItem(Item itemToAdd, int count)
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
