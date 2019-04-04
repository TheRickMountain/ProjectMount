using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Item
    {
        public const int STICK = 0;
        public const int FLINT = 1;
        public const int BERRY = 2;
        public const int FLINT_KNIFE = 3;
        public const int WOODEN_SPEAR = 4;
        public const int HAY = 5;
        public const int WOOD = 6;
        public const int STONE = 7;
        public const int FISH = 8;
        public const int WHEAT = 9;
        public const int WHEAT_SEED = 10;
        public const int BARLEY = 11;
        public const int BARLEY_SEED = 12;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool Stackable { get; private set; }
        public MyTexture Icon { get; private set; }
        public bool Consumable { get; private set; }
        public int FoodValue { get; private set; }
        public bool Weapon { get; private set; }

        public Item(int id, string name, bool stackable, bool consumable, int foodValue, bool weapon)
        {
            Id = id;
            Name = name;
            Stackable = stackable;
            Icon = GamePlayState.NewTileset[id];
            Consumable = consumable;
            FoodValue = foodValue;
            Weapon = weapon;
        }

    }
}
