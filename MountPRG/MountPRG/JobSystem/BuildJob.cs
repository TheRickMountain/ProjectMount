using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class BuildJob : Job
    {

        public BuildJob(Tile tile) : base(tile, JobType.BUILD)
        {

        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            if (TargetTile.Entity.Get<BuildingCmp>().ReadyToBuild)
            {
                TargetTile.Walkable = true;
                PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
                TargetTile.Walkable = false;
                if (pathAStar.Length != -1)
                {
                    settler.SettlerState = SettlerState.WORKING;

                    List<Tile> path = pathAStar.GetList();

                    if (path.Count > 1)
                        settler.Tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                    settler.Tasks.Add(new Task(TaskType.BUILD, TargetTile, 5));
                    settler.CurrentTask = settler.Tasks[0];
                }
                else
                {
                    settler.NextJob();
                }
            }
            else
            {
                settler.NextJob();
            }
        }

    }
}
