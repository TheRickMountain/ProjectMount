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

        public const int STICK = 0;
        public const int FLINT = 1;
        public const int BERRY = 2;
        public const int FLINT_KNIFE = 3;
        public const int WOODEN_SPEAR = 4;
        public const int HAY = 5;

        private static List<Item> items = new List<Item>();

        public ItemDatabase()
        {
            items.Add(new Item(STICK, "stick", true, false, false));
            items.Add(new Item(FLINT, "stone", true, false, false));
            items.Add(new Item(BERRY, "berry", true, true, false));
            items.Add(new Item(FLINT_KNIFE, "flint_knife", false, false, true));
            items.Add(new Item(WOODEN_SPEAR, "wooden_spear", false, false, true));
            items.Add(new Item(HAY, "hay", true, false, false));
        }

        public static Item GetItemById(int id)
        {
            return items[id];
        }

    }

    public class Item
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool Stackable { get; private set; }
        public Texture2D Texture { get; private set; }
        public bool Consumable { get; private set; }
        public bool Weapon { get; private set; }

        public Item(int id, string name, bool stackable, bool consumable, bool weapon)
        {
            Id = id;
            Name = name;
            Stackable = stackable;
            Texture = ResourceBank.Sprites[name];
            Consumable = consumable;
            Weapon = weapon;
        }
    }
}
