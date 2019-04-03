using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class HaulJob : Job
    {

        private int stockpileCount = 0;
        private int stockpileTileCount = 0;

        public HaulJob(Tile tile) : base(tile, JobType.HAUL)
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
                case TaskType.TAKE:
                    {
                        Tile tile = CurrentTask.Tile;
                        tile.Selected = false;

                        settler.Cargo = tile.Item;
                        settler.CargoCount = 1;

                        tile.ItemToRemoveCount--;
                        tile.ItemCount--;
                        tile.ItemToAddCount--;

                        if (tile.ItemCount == 0)
                        {
                            tile.Item = null;
                            tile.BuildingLayerId = -1;

                            if (tile.ItemToAddCount == 0)
                                tile.ItemToAdd = null;

                            tile.ItemToRemoveCount = 0;
                        }

                        Tasks.Remove(CurrentTask);
                        CurrentTask = Tasks[0];
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
            if (GamePlayState.Stockpiles.Count > 0 && settler.IsWalkable(TargetTile))
            {
                Tile stockpileTile = GamePlayState.Stockpiles[stockpileCount].GetTiles()[stockpileTileCount];

                if (StockpileIsAvailableFor(stockpileTile, TargetTile.Item) && settler.IsWalkable(stockpileTile))
                {
                    // Тайл получает информацию о том какой предмет туда нужно добавить и сколько
                    stockpileTile.ItemToAdd = TargetTile.Item;
                    stockpileTile.ItemToAddCount++;

                    // делаем работу текущей для данного поселенца
                    settler.SettlerState = SettlerState.WORKING;

                    Tasks.Add(new Task(TaskType.MOVE_TO_TILE, TargetTile, 0));
                    Tasks.Add(new Task(TaskType.TAKE, TargetTile, 0));
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
            if (stockpileTileCount >= GamePlayState.Stockpiles[stockpileCount].GetTiles().Count)
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
