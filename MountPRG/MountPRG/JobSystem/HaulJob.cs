using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class HaulJob : Job
    {

        private int stockpileCount = 0;
        private int stockpileTileX = 0;
        private int stockpileTileY = 0;

        public HaulJob(Tile tile) : base(tile, JobType.HAUL)
        {

        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            if (GamePlayState.StockpileList.Count > 0 && settler.IsWalkable(TargetTile))
            {
                Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                if (StockpileIsAvailableFor(stockpileTile, TargetTile.Item) && settler.IsWalkable(stockpileTile))
                {
                    // Тайл получает информацию о том какой предмет туда нужно добавить и сколько
                    stockpileTile.ItemToAdd = TargetTile.Item;
                    stockpileTile.ItemToAddCount++;

                    // делаем работу текущей для данного поселенца
                    settler.SettlerState = SettlerState.WORKING;

                    settler.Tasks.Add(new Task(TaskType.MOVE, TargetTile, 0));
                    settler.Tasks.Add(new Task(TaskType.TAKE, TargetTile, 0));
                    settler.Tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                    settler.Tasks.Add(new Task(TaskType.PUT, stockpileTile, 0));
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
