using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum JobType
    {
        HARVEST,
        CHOP,
        MINE,
        HAUL,
        STOCKPILE,
        BUILD,
        FISH,
        PLANT,
        SUPPLY,
        NONE
    }

    public enum JobState
    {
        AVAILABLE, 
        UNAVAILABLE,
        COMPLETED
    }

    public class Job
    {
        public Item Item { get; private set; }
        public Tile Tile { get; private set; }
        public JobType JobType { get; private set; }
        public JobState JobState { get; protected set; }
        public Settler Owner { get; set; }

        public Job(Tile tile, JobType jobType)
            : this(null, tile, jobType)
        {
            
        }

        public Job(Item item, Tile tile, JobType jobType)
        {
            Item = item;
            Tile = tile;
            JobType = jobType;
        }

        public virtual void DoJob(SettlerControllerCmp settler)
        {
            
        }

        public virtual bool CheckJob(SettlerControllerCmp settler)
        {
            return false;
        }

    }
}
