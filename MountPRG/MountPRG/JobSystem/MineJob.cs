using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class MineJob : Job
    {

        public MineJob(Tile tile) : base(tile, JobType.MINE)
        {

        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            TargetTile.Walkable = true;
            PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
            TargetTile.Walkable = false;
            if (pathAStar.Length != -1)
            {
                List<Tile> path = pathAStar.GetList();

                if (path.Count > 1)
                    settler.Tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                settler.Tasks.Add(new Task(TaskType.MINE, TargetTile, 2));
                settler.CurrentTask = settler.Tasks[0];

                settler.SettlerState = SettlerState.WORKING;
            }
            else
            {
                settler.NextJob();
            }
        }

    }
}
