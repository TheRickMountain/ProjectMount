using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class NewTileset
    {
        private MyTexture[,] tiles;

        public NewTileset(Texture2D texture, int tileWidth, int tileHeight)
        {
            Texture = texture;
            TileWidth = tileWidth;
            TileHeight = TileHeight;

            tiles = new MyTexture[Texture.Width / tileWidth, Texture.Height / tileHeight];
            for (int x = 0; x < Texture.Width / tileWidth; x++)
                for (int y = 0; y < Texture.Height / tileHeight; y++)
                    tiles[x, y] = new MyTexture(texture, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
        }

        public Texture2D Texture
        {
            get; private set;
        }

        public int TileWidth
        {
            get; private set;
        }

        public int TileHeight
        {
            get; private set;
        }

        public MyTexture this[int x, int y]
        {
            get
            {
                return tiles[x, y];
            }
        }

        public MyTexture this[int index]
        {
            get
            {
                if (index < 0)
                    return null;
                else
                    return tiles[index % tiles.GetLength(0), index / tiles.GetLength(0)];
            }
        }
    }
}
