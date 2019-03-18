using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    class Building : Component
    {

        public int Rows
        {
            get; private set;
        }

        public int Columns
        {
            get; private set;
        }

        public Building(int rows, int columns) : base(false, false)
        {
            Rows = rows;
            Columns = columns;
        }

    }
}
