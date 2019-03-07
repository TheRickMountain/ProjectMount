using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public class ActionPanelGUI : IGUI
    {
        private UI textPanel;

        private List<Slot> slots;

        public ActionPanelGUI(int count, bool active) : base(active)
        {
            textPanel = new UI();

            slots = new List<Slot>();
            int xStart = (Game1.ScreenRectangle.Width / 2) - ((count * GUIManager.SLOT_SIZE + (count - 1) * GUIManager.OFFSET) / 2);
            int yStart = Game1.ScreenRectangle.Height - GUIManager.SLOT_SIZE - GUIManager.OFFSET;
            for (int i = 0; i < count; i++)
            {
                Slot slot = new Slot(ResourceBank.SpellSlotTexture, GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
                slot.X = xStart + i * GUIManager.SLOT_SIZE + i * GUIManager.OFFSET;
                slot.Y = yStart;
                slots.Add(slot);
            }

            textPanel.InnerWidth = 200;
            textPanel.InnerHeight = 50;
            textPanel.X = 0;
            textPanel.Y = Game1.ScreenRectangle.Height - textPanel.Height;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPanel.Draw(spriteBatch);
            spriteBatch.DrawString(ResourceBank.Font, "You gained 25 exp", new Vector2(textPanel.InnerX, textPanel.InnerY), Color.White);
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }
        }

        /*public int AddItem(Item itemToAdd, int count)
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

        public void SetSelectedSlot(Slot slot)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetSelected(false);
            }

            slot.SetSelected(true);
            SelectedSlot = slot;
        }

        public Slot GetIntersectedSlot()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Intersects(InputManager.GetX(), InputManager.GetY()))
                {
                    return slots[i];
                }       
            }
               
            return null;
        }*/
    }
}
