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

        public static Texture2D SettlerTexture;
        public static Texture2D TilesetTexture;
        public static Texture2D SelectorTexture;
        public static Texture2D SettlerAvatarTexture;
        public static Texture2D SlotTexture;
        public static Texture2D WoodTexture;
        public static Texture2D ChestTexture;
        public static Texture2D ProgressBarTexture;
        public static Texture2D CrossTexture;
        public static Texture2D CharactersTexture;
        public static Texture2D TreeTexture;

        public static void CreateInstance(ContentManager content)
        {
            new TextureBank(content);
        }

        private TextureBank(ContentManager content)
        {
            SettlerTexture = content.Load<Texture2D>(@"human");
            TilesetTexture = content.Load<Texture2D>(@"tileset");
            SelectorTexture = content.Load<Texture2D>(@"selector");
            SettlerAvatarTexture = content.Load<Texture2D>(@"settler_avatar");
            SlotTexture = content.Load<Texture2D>(@"slot");
            WoodTexture = content.Load<Texture2D>(@"stick");
            ChestTexture = content.Load<Texture2D>(@"chest");
            ProgressBarTexture = content.Load<Texture2D>(@"progressBar");
            CrossTexture = content.Load<Texture2D>(@"cross");
            CharactersTexture = content.Load<Texture2D>(@"characters");
            TreeTexture = content.Load<Texture2D>(@"tree");
        }

    }
}
