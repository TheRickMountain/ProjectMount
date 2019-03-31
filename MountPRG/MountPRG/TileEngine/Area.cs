using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum AreaType
    {
        STOCKPILE,
        FARM,
        NONE
    }

    public class Area
    {

        public AreaType AreaType { get; private set; } 

        public int Num { get; private set; }

        public Area()
        {
            Set(AreaType.NONE, -1);
        }

        public void Set(AreaType areaType, int num)
        {
            AreaType = areaType;
            Num = num;
        }

    }
}
