using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class FarmList
    {

        private Dictionary<int, Tile[,]> farms;
        
        public FarmList()
        {
            farms = new Dictionary<int, Tile[,]>();
        }

        public void Add(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
                for (int j = 0; j < tiles.GetLength(1); j++)
                    tiles[i, j].Area.Set(AreaType.FARM, Count);

            farms.Add(Count, tiles);
        }

        public Tile[,] Get(int val)
        {
            return farms[val];
        }

        public int Count
        {
            get { return farms.Count; }
        }

    }
}
