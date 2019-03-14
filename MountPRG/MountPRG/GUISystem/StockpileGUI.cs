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
    public class StockpileGUI : IGUI
    {

        private List<Slot> slots = new List<Slot>();
        private Tile[,] tiles;

        private UI background;

        public StockpileGUI(bool active) : base(active)
        {
            background = new UI();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Slot slot = slots[i * tiles.GetLength(1) + j];
                    Tile tile = tiles[i, j];
                    if (tile.Entity != null)
                        slot.AddItem(tile.Entity.Get<Gatherable>().Item, tile.EntityCount);
                    else
                        slot.Clear();
                }
            }

            if (InputManager.GetKeyDown(Keys.E))
            {
                Active = false;
                slots.Clear();
                tiles = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }

        }

        public void Open(Tile[,] tiles)
        {
            this.tiles = tiles;

            background.InnerWidth = tiles.GetLength(0) * GUIManager.SLOT_SIZE + (tiles.GetLength(0) - 1) * GUIManager.OFFSET;
            background.InnerHeight = tiles.GetLength(1) * GUIManager.SLOT_SIZE + (tiles.GetLength(1) - 1) * GUIManager.OFFSET;

            background.X = Game1.ScreenRectangle.Width- background.Width;
            background.Y = Game1.ScreenRectangle.Height - background.Height;

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Slot slot = new Slot(ResourceBank.Sprites["slot"], GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true);
                    slot.X = background.InnerX + i * GUIManager.SLOT_SIZE + i * GUIManager.OFFSET;
                    slot.Y = background.InnerY + j * GUIManager.SLOT_SIZE + j * GUIManager.OFFSET;
                    Tile tile = tiles[i, j];
                    if (tile.Entity != null)
                    {
                        slot.AddItem(tile.Entity.Get<Gatherable>().Item, tile.EntityCount);
                    }
                    slots.Add(slot);
                }
            }

            Active = true;
        }
    }
}
