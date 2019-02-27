using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Storage : Component
    {


        public int Rows
        {
            get;
            private set;
        }

        public int Columns
        {
            get;
            private set;
        }

        public int[] Items
        {
            get; private set;
        }

        public int[] Count
        {
            get; private set;
        }

        public Storage(int rows, int columns)
            : base(false, false)
        {
            Rows = rows;
            Columns = columns;
            Items = new int[rows * columns];
            Count = new int[rows * columns];
            MathUtils.Populate(Items, -1);
            MathUtils.Populate(Count, 0);
        }

    }
}
