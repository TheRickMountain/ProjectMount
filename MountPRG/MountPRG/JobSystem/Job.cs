﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

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
        public Tile TargetTile { get; private set; }
        public JobType JobType { get; private set; }
        public JobState JobState { get; set; }
        
        public Job(Tile tile, JobType jobType)
            : this(null, tile, jobType)
        {
            
        }

        public Job(Item item, Tile tile, JobType jobType)
        {
            Item = item;
            TargetTile = tile;
            JobType = jobType;
        }

        public virtual void CheckJob(SettlerControllerCmp settler)
        {

        }
    }
}
