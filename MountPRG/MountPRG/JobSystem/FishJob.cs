using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class FishJob : Job
    {
        private int stockpileCount = 0;
        private int stockpileTileX = 0;
        private int stockpileTileY = 0;

        public FishJob(Tile tile) : base(tile, JobType.FISH)
        {

        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            TargetTile.Walkable = true;
            PathAStar pathAStar = new PathAStar(settler.CurrentTile, TargetTile, TargetTile.Tilemap.GetTileGraph().Nodes, TargetTile.Tilemap);
            TargetTile.Walkable = false;
            if (pathAStar.Length != -1 && GamePlayState.StockpileList.Count > 0)
            {
                Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                if (StockpileIsAvailableFor(stockpileTile, ItemDatabase.GetItemById(TileMap.FISH)) &&
                    settler.IsWalkable(stockpileTile))
                {
                    stockpileTile.ItemToAdd = ItemDatabase.GetItemById(TileMap.FISH);
                    stockpileTile.ItemToAddCount++;

                    settler.SettlerState = SettlerState.WORKING;

                    List<Tile> path = pathAStar.GetList();

                    if (path.Count > 1)
                        settler.Tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                    settler.Tasks.Add(new Task(TaskType.FISH, TargetTile, 5));
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
