using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Stockpile
    {

        private List<Tile> tiles;

        public Stockpile(List<Tile> tiles)
        {
            this.tiles = tiles;
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].Stockpile = this;
        }

        public void AddTile(Tile tile)
        {
            tile.Stockpile = this;
            tiles.Add(tile);
        }

        public List<Tile> GetTiles()
        {
            return tiles;
        }

        public bool Contains(Item item)
        {
            for(int i = 0; i < tiles.Count; i++)
            {
                Tile tile = tiles[i];
                if (tile.Item == item)
                    return true;
            }

            return false;
        }

    }
}
