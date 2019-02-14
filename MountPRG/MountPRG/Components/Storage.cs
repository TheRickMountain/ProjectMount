using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Storage : Component
    {

        private Dictionary<Item, int> items;

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

        public Storage(int rows, int columns)
            : base(false, false)
        {
            Rows = rows;
            Columns = columns;
            items = new Dictionary<Item, int>(Rows * Columns);
        }

    }
}
