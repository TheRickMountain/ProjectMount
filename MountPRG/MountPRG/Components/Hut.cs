using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Hut : Component
    {

        public Settler Owner
        {
            get; private set;
        }

        public Hut() : base(false, false)
        {

        }

        public void SetOwner(Settler settler)
        {
            Owner = settler;
        }

    }
}
