using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class TileSet
    {
        private Rectangle[] sourceRectangles;

        public int TilesWide
        {
            get; private set;
        }

        public int TilesHigh
        {
            get; private set;
        }

        public Texture2D Texture
        {
            get; private set;
        }

        public string TextureName
        {
            get; set;
        }

        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        public TileSet(Texture2D texture)
        {
            Texture = texture;

            TilesWide = texture.Width / TileMap.TILE_SIZE;
            TilesHigh = texture.Height / TileMap.TILE_SIZE;

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
            {
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileMap.TILE_SIZE,
                        y * TileMap.TILE_SIZE,
                        TileMap.TILE_SIZE,
                        TileMap.TILE_SIZE);
                    tile++;
                }
            }
        }

    }
}
