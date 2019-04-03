using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum Plant
    {
        WHEAT,
        BARLEY,
        NONE
    }

    public class Farm
    {
        private List<Tile> tiles;

        public Plant TargetPlant { get; private set; }

        public Farm(List<Tile> tiles)
        {
            this.tiles = tiles;
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].Farm = this;
            TargetPlant = Plant.NONE;
        }

        public void AddTile(Tile tile)
        {
            tile.Farm = this;
            tiles.Add(tile);    
        }

        public void SetTargetPlant(Plant plant)
        {
            if (TargetPlant != plant)
            {
                TargetPlant = plant;

                switch (plant)
                {
                    case Plant.WHEAT:
                        for (int i = 0; i < tiles.Count; i++)
                        {
                            GamePlayState.JobList.Add(new PlantJob(ItemDatabase.GetItemById(TileMap.WHEAT_SEED), tiles[i]));
                        }
                        break;
                    case Plant.BARLEY:
                        for (int i = 0; i < tiles.Count; i++)
                        {
                            GamePlayState.JobList.Add(new PlantJob(ItemDatabase.GetItemById(TileMap.BARLEY_SEED), tiles[i]));
                        }
                        break;
                    case Plant.NONE:
                        //TODO: cancel all active plant work
                        break;
                }
            }
        }

        public List<Tile> GetTiles()
        {
            return tiles;
        }

    }
}
