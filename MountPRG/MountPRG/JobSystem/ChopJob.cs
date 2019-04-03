using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class ChopJob : Job
    {
        
        public ChopJob(Tile tile) 
            : base(tile, JobType.CHOP)
        {
            JobState = JobState.AVAILABLE;
        }

        public override void DoJob(SettlerControllerCmp settler)
        {
            
        }

        public override bool CheckJob(SettlerControllerCmp settler)
        {
            return false;
        }
    }
}
