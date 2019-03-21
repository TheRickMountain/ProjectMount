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
            items.Add(TileMap.STICK,        new Item(TileMap.STICK,        "stick", true, false, false));
            items.Add(TileMap.FLINT,        new Item(TileMap.FLINT,        "stone", true, false, false));
            items.Add(TileMap.BERRY,        new Item(TileMap.BERRY,        "berry", true, true, false));
            items.Add(TileMap.FLINT_KNIFE,  new Item(TileMap.FLINT_KNIFE,  "flint_knife", false, false, true));
            items.Add(TileMap.WOODEN_SPEAR, new Item(TileMap.WOODEN_SPEAR, "wooden_spear", false, false, true));
            items.Add(TileMap.HAY,          new Item(TileMap.HAY,          "hay", true, false, false));
            items.Add(TileMap.WOOD,         new Item(TileMap.WOOD,         "wood", true, false, false));
            items.Add(TileMap.STONE,        new Item(TileMap.STONE,        "stone", true, false, false));
            items.Add(TileMap.FISH,         new Item(TileMap.FISH,         "fish", true, false, false));
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
        public bool Weapon { get; private set; }

        public Item(int id, string name, bool stackable, bool consumable, bool weapon)
        {
            Id = id;
            Name = name;
            Stackable = stackable;
            Sprite = new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[id], 16, 16, true);
            Consumable = consumable;
            Weapon = weapon;
        }

    }
}
