using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class StockpileList
    {

        private Dictionary<int, Tile[,]> stockpiles;
        
        public StockpileList()
        {
            stockpiles = new Dictionary<int, Tile[,]>();
        }

        public void Add(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
                for (int j = 0; j < tiles.GetLength(1); j++)
                    tiles[i, j].Stockpile = Count;

            stockpiles.Add(Count, tiles);
        }

        public Tile[,] Get(int val)
        {
            return stockpiles[val];
        }

        public int Count
        {
            get { return stockpiles.Count; }
        }

    }
}
