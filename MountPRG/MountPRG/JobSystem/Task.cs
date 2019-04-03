using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum TaskType
    {
        MOVE_TO_TILE,
        MINE,
        CHOP,
        TAKE,
        PUT,
        HARVEST,
        FISH,
        EAT,
        SLEEP,
        PLOW,
        PLANT,
        BUILD,
        MOVE_TO_STOCKPILE
    }

    public class Task
    {

        public TaskType TaskType;
        public Tile Tile;
        public float Time;

        public Task(TaskType taskType, Tile tile, float time)
        {
            TaskType = taskType;
            Tile = tile;
            Time = time;
        }

    }
}
