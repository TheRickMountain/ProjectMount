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
        BUILDING,
        FISH,
        NONE
    }

    public class Job
    {

        public Tile Tile { get; private set; }
        public float JobTime { get; private set; }
        public JobType JobType { get; private set; }
        public Settler Owner { get; set; }

        public Job(Tile tile, JobType jobType, float jobTime = 1f)
        {
            Tile = tile;
            JobType = jobType;
            JobTime = jobTime;
        }

    }
}
