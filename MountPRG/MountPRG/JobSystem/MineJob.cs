using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class MineJob : Job
    {

        public MineJob(Tile tile) : base(tile, JobType.MINE)
        {

        }

        public override void DoJob(SettlerControllerCmp settler, GameTime gameTime)
        {
            switch(CurrentTask.TaskType)
            {
                case TaskType.MOVE_TO_TILE:
                    {
                        if (settler.RebuildPath && settler.CurrentTile.Equals(settler.NextTile))
                        {
                            TargetTile.walkable = true;
                            PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
                            TargetTile.walkable = false;
                            if (pathAStar.Length != -1)
                            {
                                List<Tile> path = pathAStar.GetList();
                                if (path.Count > 1)
                                {
                                    CurrentTask.Tile = path[path.Count - 2];
                                    settler.SetDestTile(CurrentTask.Tile);
                                }
                                else
                                {
                                    Tasks.Remove(CurrentTask);
                                    CurrentTask = Tasks[0];
                                    settler.PathAStar = null;
                                    settler.AnimationState = AnimationState.IDLE;
                                }
                            }
                            else
                            {
                                JobState = JobState.AVAILABLE;
                                settler.SettlerState = SettlerState.WAITING;
                                settler.AnimationState = AnimationState.IDLE;
                                settler.MyJob = null;
                                settler.PathAStar = null;
                            }

                            settler.RebuildPath = false;
                        }
                        else
                        {
                            if (settler.MoveTo(CurrentTask.Tile, gameTime))
                            {
                                Tasks.Remove(CurrentTask);
                                CurrentTask = Tasks[0];
                            }
                        }
                    }
                    break;
                case TaskType.MINE:
                    {
                        if (settler.WorkProgress(2, gameTime))
                        {
                            CurrentTask.Tile.Selected = false;
                            CurrentTask.Tile.AddItem(ItemDatabase.GetItemById(TileMap.STONE), 1);
                            Tasks.Clear();

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
            TargetTile.Walkable = true;
            PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
            TargetTile.Walkable = false;
            if (pathAStar.Length != -1)
            {
                List<Tile> path = pathAStar.GetList();

                if (path.Count > 1)
                    Tasks.Add(new Task(TaskType.MOVE_TO_TILE, path[path.Count - 2], 0));

                Tasks.Add(new Task(TaskType.MINE, TargetTile, 2));
                CurrentTask = Tasks[0];

                settler.SettlerState = SettlerState.WORKING;
            }
            else
            {
                settler.NextJob();
            }
        }

    }
}
