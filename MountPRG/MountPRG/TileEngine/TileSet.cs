using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG.TileEngine
{
    public class TileSet
    {
        private int tilesWide;
        private int tilesHigh;
        private int tileWidth;
        private int tileHeight;

        private Texture2D texture;
        private string imageName;
        private Rectangle[] sourceRectangles;

        public int TilesWide
        {
            get { return tilesWide; }
        }

        public int TilesHigh
        {
            get { return tilesHigh; }
        }

        public int TileWidth
        {
            get { return tileWidth; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public string TextureName
        {
            get { return imageName; }
            set { imageName = value; }
        }

        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        public TileSet(Texture2D texture, int tileWidth, int tileHeight)
        {
            this.texture = texture;

            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            this.tilesWide = texture.Width / tileWidth;
            this.tilesHigh = texture.Height / tileHeight;

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
            {
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight);
                    tile++;
                }
            }
        }

    }
}
