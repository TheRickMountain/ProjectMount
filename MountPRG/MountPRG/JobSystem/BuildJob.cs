using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class BuildJob : Job
    {

        public BuildJob(Tile tile) : base(tile, JobType.BUILD)
        {

        }

        public override void DoJob(SettlerControllerCmp settler, GameTime gameTime)
        {
            switch (CurrentTask.TaskType)
            {
                case TaskType.MOVE_TO_TILE:
                    {
                        if (settler.MoveTo(CurrentTask.Tile, gameTime))
                        {
                            Tasks.Remove(CurrentTask);
                            CurrentTask = Tasks[0];
                        }
                    }
                    break;
                case TaskType.BUILD:
                    {
                        if (settler.WorkProgress(CurrentTask.Time, gameTime))
                        {
                            CurrentTask.Tile.Entity.Visible = true;

                            for (int i = 0; i < CurrentTask.Tile.Entity.Get<BuildingCmp>().Tiles.Count; i++)
                            {
                                CurrentTask.Tile.Entity.Get<BuildingCmp>().Tiles[i].BuildingLayerId = -1;
                            }

                            JobState = JobState.COMPLETED;
                            settler.SettlerState = SettlerState.WAITING;
                            settler.MyJob = null;
                        }                 
                    }
                    break;
            }
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
                        Tasks.Add(new Task(TaskType.MOVE_TO_TILE, path[path.Count - 2], 0));

                    Tasks.Add(new Task(TaskType.BUILD, TargetTile, 5));
                    CurrentTask = Tasks[0];
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
