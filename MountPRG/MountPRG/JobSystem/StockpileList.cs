using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class StockpileList
    {

        private Dictionary<int, List<Tile>> stockpiles;

        public StockpileList()
        {
            stockpiles = new Dictionary<int, List<Tile>>();
        }

        public void Add(List<Tile> tiles)
        {
            Console.WriteLine("Stockpile has added");
            stockpiles.Add(Count, tiles);
        }

        public List<Tile> Get(int val)
        {
            return stockpiles[val];
        }

        public Tile GetEmptyTileFrom(int val)
        {
            List<Tile> tiles = stockpiles[val];
            for(int i = 0; i < tiles.Count; i++)
            {
                Tile tile = tiles[i];
                if (tile.Entity == null)
                    return tile;
            }

            return null;
        }

        public int Count
        {
            get { return stockpiles.Count; }
        }

    }
}
