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
                Slot slot = new Slot(TextureBank.SlotTexture, 50, 50, true);
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

        public bool AddItem(Item item, int count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (!slots[i].HasItem)
                {
                    slots[i].AddItem(item, count);
                    return true;
                }
            }
            return false;
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
