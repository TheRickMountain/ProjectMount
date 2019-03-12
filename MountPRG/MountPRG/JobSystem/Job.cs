using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum JobType
    {
        GATHER,
        CUT,
        MINE,
        BUILD,
        STORAGE,
        NONE
    }

    public class Job
    {

        public Tile Tile { get; private set; }
        public float JobTime { get; private set; }
        public JobType JobType { get; private set; }

        public Job(Tile tile, JobType jobType, float jobTime = 1f)
        {
            Tile = tile;
            JobType = jobType;
            JobTime = jobTime;
        }

    }
}
