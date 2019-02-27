using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class TextureBank
    {
        
        public static Texture2D TilesetTexture;
        public static Texture2D SelectorTexture;
        public static Texture2D SlotTexture;
        public static Texture2D WoodTexture;
        public static Texture2D StoneTexture;
        public static Texture2D ChestTexture;
        public static Texture2D CharactersTexture;
        public static Texture2D TreeTexture;
        public static Texture2D DayNightCircleTexture;
        public static Texture2D DayNightArrowTexture;
        public static Texture2D BerryTexture;

        public static SpriteFont Font;

        public static void CreateInstance(ContentManager content)
        {
            new TextureBank(content);
        }

        private TextureBank(ContentManager content)
        {
            TilesetTexture = content.Load<Texture2D>(@"tileset");
            SelectorTexture = content.Load<Texture2D>(@"selector");
            SlotTexture = content.Load<Texture2D>(@"slot");
            WoodTexture = content.Load<Texture2D>(@"stick");
            StoneTexture = content.Load<Texture2D>(@"stone");
            ChestTexture = content.Load<Texture2D>(@"chest");
            CharactersTexture = content.Load<Texture2D>(@"characters");
            TreeTexture = content.Load<Texture2D>(@"tree");
            DayNightCircleTexture = content.Load<Texture2D>(@"day_night");
            DayNightArrowTexture = content.Load<Texture2D>(@"arrow");
            BerryTexture = content.Load<Texture2D>(@"berry");

            Font = content.Load<SpriteFont>(@"mountFont");
        }

    }
}
