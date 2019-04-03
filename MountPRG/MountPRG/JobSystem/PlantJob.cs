using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class PlantJob : Job
    {
        private int stockpileCount = 0;
        private int stockpileTileX = 0;
        private int stockpileTileY = 0;

        public PlantJob(Tile tile) : base(tile, JobType.PLANT)
        {

        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            // Тайл не засеян
            if (TargetTile.BuildingLayerId == -1 && settler.IsWalkable(TargetTile))
            {
                // Есть ли на складе необходимые семена
                if (GamePlayState.StockpileList.HasItem(ItemDatabase.GetItemById(TileMap.WHEAT_SEED)))
                {
                    Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                    // Находим тайл который содержит необходимые нам никем не занятые семена 
                    if (stockpileTile.Item == ItemDatabase.GetItemById(TileMap.WHEAT_SEED)
                        && ((stockpileTile.ItemCount - stockpileTile.ItemToRemoveCount) > 0)
                        && settler.IsWalkable(stockpileTile))
                    {
                        stockpileTile.ItemToRemoveCount++;

                        settler.SettlerState = SettlerState.WORKING;

                        settler.Tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                        settler.Tasks.Add(new Task(TaskType.TAKE, stockpileTile, 0));
                        settler.Tasks.Add(new Task(TaskType.MOVE, TargetTile, 0));
                        settler.Tasks.Add(new Task(TaskType.PLOW, TargetTile, 2));
                        settler.Tasks.Add(new Task(TaskType.PLANT, TargetTile, 2));
                        settler.CurrentTask = settler.Tasks[0];

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
            stockpileTileX = 0;
            stockpileTileY = 0;
        }

        private bool NextStockpileTile()
        {
            stockpileTileX++;
            if (stockpileTileX == GamePlayState.StockpileList.Get(stockpileCount).GetLength(0))
            {
                stockpileTileY++;

                if (stockpileTileY == GamePlayState.StockpileList.Get(stockpileCount).GetLength(1))
                {
                    stockpileCount++;
                    if (stockpileCount == GamePlayState.StockpileList.Count)
                    {
                        ResetStockpileCounter();
                        return false;
                    }
                    stockpileTileY = 0;
                }
                stockpileTileX = 0;
            }

            return true;
        }

    }
}
