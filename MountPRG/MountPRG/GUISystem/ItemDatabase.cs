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
            items.Add(TileMap.STICK,        new Item(TileMap.STICK,        "stick", true, false,         0, false));
            items.Add(TileMap.FLINT,        new Item(TileMap.FLINT,        "stone", true, false,         0, false));
            items.Add(TileMap.BERRY,        new Item(TileMap.BERRY,        "berry", true, true,          10, false));
            items.Add(TileMap.FLINT_KNIFE,  new Item(TileMap.FLINT_KNIFE,  "flint_knife", false, false,  0, true));
            items.Add(TileMap.WOODEN_SPEAR, new Item(TileMap.WOODEN_SPEAR, "wooden_spear", false, false, 0, true));
            items.Add(TileMap.HAY,          new Item(TileMap.HAY,          "hay", true, false,           0, false));
            items.Add(TileMap.WOOD,         new Item(TileMap.WOOD,         "wood", true, false,          0, false));
            items.Add(TileMap.STONE,        new Item(TileMap.STONE,        "stone", true, false,         0, false));
            items.Add(TileMap.FISH,         new Item(TileMap.FISH,         "fish", true, false,          20, false));
        }

        public static Item GetItemById(int id)
        {
            if (items.ContainsKey(id))
                return items[id];

            return null;
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
