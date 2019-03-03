using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Mineable : Component
    {

        public Item Resource
        {
            get; private set;
        }

        public int Health
        {
            get; private set;
        }

        public Mineable(Item resource, int health)
            : base(false, false)
        {
            Resource = resource;
            Health = health;
        }

        public void ReduceHealth(int damage)
        {
            Health -= damage;
            Health = Health < 0 ? 0 : Health;
            ResourceBank.ChopSong.Play();
        }

    }
}
