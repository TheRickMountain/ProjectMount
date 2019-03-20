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
    public class StockpileUI : UI
    {

        private List<SlotUI> slots = new List<SlotUI>();
        private Tile[,] tiles;

        private PanelUI background;

        public StockpileUI()
        {
            background = new PanelUI();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    SlotUI slot = slots[i * tiles.GetLength(1) + j];
                    Tile tile = tiles[i, j];
                    if (tile.Item != null)
                        slot.AddItem(tile.Item, tile.ItemCount);
                    else
                        slot.Clear();
                }
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
                    SlotUI slot = new SlotUI(ResourceBank.Sprites["slot"], GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE);
                    slot.X = background.InnerX + i * GUIManager.SLOT_SIZE + i * GUIManager.OFFSET;
                    slot.Y = background.InnerY + j * GUIManager.SLOT_SIZE + j * GUIManager.OFFSET;

                    Tile tile = tiles[i, j];
                    if (tile.Item != null)
                        slot.AddItem(tile.Item, tile.ItemCount);

                    slots.Add(slot);
                }
            }

            Active = true;
        }

        public void Close()
        {
            Active = false;

            slots.Clear();
            tiles = null;
        }

        public override bool Intersects(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
