using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class FishJob : Job
    {
        private int stockpileCount = 0;
        private int stockpileTileCount = 0;

        public FishJob(Tile tile) : base(tile, JobType.FISH)
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
                case TaskType.FISH:
                    {
                        if (settler.WorkProgress(CurrentTask.Time, gameTime))
                        {
                            Tile tile = CurrentTask.Tile;
                            tile.Selected = false;
                            settler.Cargo = ItemDatabase.GetItemById(TileMap.FISH);
                            settler.CargoCount = 1;

                            Tasks.Remove(CurrentTask);
                            CurrentTask = Tasks[0];
                        }
                    }
                    break;
                case TaskType.MOVE_TO_STOCKPILE:
                    {
                        if (settler.MoveTo(CurrentTask.Tile, gameTime))
                        {
                            Tasks.Remove(CurrentTask);
                            CurrentTask = Tasks[0];
                        }
                    }
                    break;
                case TaskType.PUT:
                    {
                        Tile tile = CurrentTask.Tile;
                        if (tile.Entity != null)
                        {
                            BuildingCmp building = tile.Entity.Get<BuildingCmp>();
                            building.AddItem(settler.Cargo, settler.CargoCount);
                        }
                        else
                        {
                            tile.Item = settler.Cargo;
                            tile.ItemCount++;
                            tile.ItemToAdd = settler.Cargo;
                            tile.BuildingLayerId = settler.Cargo.Id;
                        }

                        settler.Cargo = null;
                        settler.CargoCount = 0;

                        JobState = JobState.COMPLETED;
                        settler.SettlerState = SettlerState.WAITING;
                        settler.MyJob = null;
                    }
                    break;
            }
        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            TargetTile.Walkable = true;
            PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
            TargetTile.Walkable = false;
            if (pathAStar.Length != -1 && GamePlayState.Stockpiles.Count > 0)
            {
                Tile stockpileTile = GamePlayState.Stockpiles[stockpileCount].GetTiles()[stockpileTileCount];

                if (StockpileIsAvailableFor(stockpileTile, ItemDatabase.GetItemById(TileMap.FISH)) &&
                    settler.IsWalkable(stockpileTile))
                {
                    stockpileTile.ItemToAdd = ItemDatabase.GetItemById(TileMap.FISH);
                    stockpileTile.ItemToAddCount++;

                    settler.SettlerState = SettlerState.WORKING;

                    List<Tile> path = pathAStar.GetList();

                    if (path.Count > 1)
                        Tasks.Add(new Task(TaskType.MOVE_TO_TILE, path[path.Count - 2], 0));

                    Tasks.Add(new Task(TaskType.FISH, TargetTile, 5));
                    Tasks.Add(new Task(TaskType.MOVE_TO_STOCKPILE, stockpileTile, 0));
                    Tasks.Add(new Task(TaskType.PUT, stockpileTile, 0));
                    CurrentTask = Tasks[0];

                    ResetStockpileCounter();
                }
                else
                {
                    if (!NextStockpileTile())
                        settler.NextJob();
                }
            }
            else
            {
                settler.NextJob();
            }
        }


        private bool StockpileIsAvailableFor(Tile tile, Item item)
        {
            if (tile.ItemToAdd == null)
                return true;
            else if (tile.ItemToAdd == item && tile.ItemToAddCount < 10)
                return true;

            return false;
        }

        private void ResetStockpileCounter()
        {
            stockpileCount = 0;
            stockpileTileCount = 0;
        }

        private bool NextStockpileTile()
        {
            stockpileTileCount++;
            if(stockpileTileCount >= GamePlayState.Stockpiles[stockpileCount].GetTiles().Count)
            {
                stockpileTileCount = 0;

                stockpileCount++;
                if (stockpileCount >= GamePlayState.Stockpiles.Count)
                {
                    ResetStockpileCounter();
                    return false;
                }
            }

            return true;
        }

    }
}
