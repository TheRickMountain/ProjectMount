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

        public const int WOOD = 0;
        public const int STONE = 1;
        public const int BERRY = 2;

        private static List<Item> items = new List<Item>();

        public ItemDatabase()
        {
            items.Add(new Item(WOOD, "wood", true, TextureBank.WoodTexture, false));
            items.Add(new Item(STONE, "stone", false, TextureBank.StoneTexture, false));
            items.Add(new Item(BERRY, "berry", false, TextureBank.BerryTexture, true));
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

        public Item(int id, string name, bool stackable, Texture2D texture, bool consumable)
        {
            Id = id;
            Name = name;
            Stackable = stackable;
            Texture = texture;
            Consumable = consumable;
        }
    }
}
