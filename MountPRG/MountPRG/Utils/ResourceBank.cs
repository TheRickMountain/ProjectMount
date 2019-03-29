using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MountPRG
{
    public class ResourceBank
    {
        public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();

        private static ResourceBank _instance;

        public static void CreateInstance(ContentManager content)
        {
            if (_instance == null)
                _instance = new ResourceBank(content);
        }

        private ResourceBank(ContentManager content)
        {
            Sprites = LoadContent<Texture2D>(content, "Sprites");
            SoundEffects = LoadContent<SoundEffect>(content, "SoundEffects");
            Fonts = LoadContent<SpriteFont>(content, "Fonts");
            Effects = LoadContent<Effect>(content, "Effects");
        }

        private Dictionary<string, T> LoadContent<T>(ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            Dictionary<string, T> result = new Dictionary<string, T>();
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);


                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }

    }
}
