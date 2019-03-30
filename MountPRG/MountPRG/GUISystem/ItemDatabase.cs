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
        private static Dictionary<int, Item> items = new Dictionary<int, Item>();

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
        }

        public static Item GetItemById(int id)
        {
            if (items.ContainsKey(id))
                return items[id];

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="stackable">Stackable</param>
        /// <param name="consumable">Consumable</param>
        /// <param name="foodValue">Food value</param>
        /// <param name="weapon">Weapon</param>
        private void AddItem(int id, string name, bool stackable, bool consumable, int foodValue, bool weapon)
        {
            items.Add(id, new Item(id, name, stackable, consumable, foodValue, weapon));
        }

    }

    public class Item
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool Stackable { get; private set; }
        public Sprite Sprite { get; private set; }
        public bool Consumable { get; private set; }
        public int FoodValue { get; private set; }
        public bool Weapon { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="stackable">Stackable</param>
        /// <param name="consumable">Consumable</param>
        /// <param name="foodValue">Food value</param>
        /// <param name="weapon">Weapon</param>
        public Item(int id, string name, bool stackable, bool consumable, int foodValue, bool weapon)
        {
            Id = id;
            Name = name;
            Stackable = stackable;
            Sprite = new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[id], 16, 16, true);
            Consumable = consumable;
            FoodValue = foodValue;
            Weapon = weapon;
        }

    }
}
