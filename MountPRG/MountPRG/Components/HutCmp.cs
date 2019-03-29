using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class HutCmp : Component
    {

        public List<Tile> Tiles;

        public Settler Owner
        {
            get; private set;
        }

        public HutCmp() : base(false, false)
        {

        }

        public void SetOwner(Settler settler)
        {
            if (Owner == null)
            {
                Owner = settler;
                Owner.Get<SettlerController>().Hut = (Hut)Parent;
            }
        }

        public void AddTiles(List<Tile> tiles)
        {
            Tiles = tiles;
        }

    }
}
