using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MountPRG
{
    public class ResourceBank
    {
        
        public static Texture2D TilesetTexture;
        public static Texture2D SelectorTexture;
        public static Texture2D SlotTexture;
        public static Texture2D SelectedSlotTexture;
        public static Texture2D WoodTexture;
        public static Texture2D StoneTexture;
        public static Texture2D ChestTexture;
        public static Texture2D CharactersTexture;
        public static Texture2D TreeTexture;
        public static Texture2D DayNightCircleTexture;
        public static Texture2D DayNightArrowTexture;
        public static Texture2D BerryTexture;
        public static Texture2D WolfTexture;
        public static Texture2D AxeTexture;
        public static Texture2D SelectionTexture;
        public static Texture2D UITexture;
        public static Texture2D SpellSlotTexture;
        public static Texture2D HeadTexture;
        public static Texture2D BodyTexture;
        public static Texture2D LegsTexture;
        public static Texture2D WeaponTexture;

        public static SpriteFont Font;

        public static SoundEffect ChopSong;
        public static SoundEffect PunchSong;

        public static void CreateInstance(ContentManager content)
        {
            new ResourceBank(content);
        }

        private ResourceBank(ContentManager content)
        {
            TilesetTexture = content.Load<Texture2D>(@"tileset");
            SelectorTexture = content.Load<Texture2D>(@"selector");
            SlotTexture = content.Load<Texture2D>(@"slot");
            SelectedSlotTexture = content.Load<Texture2D>(@"selectedSlot");
            WoodTexture = content.Load<Texture2D>(@"stick");
            StoneTexture = content.Load<Texture2D>(@"stone");
            ChestTexture = content.Load<Texture2D>(@"chest");
            CharactersTexture = content.Load<Texture2D>(@"characters");
            TreeTexture = content.Load<Texture2D>(@"tree");
            DayNightCircleTexture = content.Load<Texture2D>(@"day_night");
            DayNightArrowTexture = content.Load<Texture2D>(@"arrow");
            BerryTexture = content.Load<Texture2D>(@"berry");
            WolfTexture = content.Load<Texture2D>(@"wolf");
            AxeTexture = content.Load<Texture2D>(@"axe");
            SelectionTexture = content.Load<Texture2D>(@"selection");
            UITexture = content.Load<Texture2D>(@"ui");
            SpellSlotTexture = content.Load<Texture2D>(@"spell_slot");
            HeadTexture = content.Load<Texture2D>(@"head");
            BodyTexture = content.Load<Texture2D>(@"body");
            LegsTexture = content.Load<Texture2D>(@"legs");
            WeaponTexture = content.Load<Texture2D>(@"weapon");

            Font = content.Load<SpriteFont>(@"mountFont");

            ChopSong = content.Load<SoundEffect>(@"SoundFx\chop");
            PunchSong = content.Load<SoundEffect>(@"SoundFx\punch");
        }

    }
}
