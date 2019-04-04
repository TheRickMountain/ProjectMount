using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class ItemDatabase
    {
        private List<Item> items = new List<Item>();

        public ItemDatabase()
        {
            AddItem(TileMap.STICK, "Stick", true, false, 0, false);
            AddItem(TileMap.FLINT, "Flint", true, false, 0, false);
            AddItem(TileMap.BERRY, "Berry", true, true, 10, false);
            AddItem(TileMap.FLINT_KNIFE,  "Flint knife",  false, false, 0, true);
            AddItem(TileMap.WOODEN_SPEAR, "Wooden spear", false, false, 0, true);
            AddItem(TileMap.HAY, "Hay", true, false, 0, false);
            AddItem(TileMap.WOOD, "Wood", true, false, 0, false);
            AddItem(TileMap.STONE, "Stone", true, false, 0, false);
            AddItem(TileMap.FISH, "Fish", true, true, 20, false);
            AddItem(TileMap.WHEAT, "Wheat", true, false, 0, false);
            AddItem(TileMap.WHEAT_SEED, "Wheat seed", true, false, 0, false);
            AddItem(TileMap.BARLEY, "Barley", true, false, 0, false);
            AddItem(TileMap.BARLEY_SEED, "Barley seed", true, false, 0, false);
        }

        public Item this[int index]
        {
            get { return items[index]; }
        }

        private void AddItem(int id, string name, bool stackable, bool consumable, int foodValue, bool weapon)
        {
            items.Add(new Item(id, name, stackable, consumable, foodValue, weapon));
        }

    }
}
