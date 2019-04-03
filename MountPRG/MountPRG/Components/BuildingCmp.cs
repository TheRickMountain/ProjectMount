using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class RequiredResource
    {
        public Item Item { get; private set; }
        public int Count { get; private set; }
        public int CurrentCount { get; set; }

        public RequiredResource(Item item, int count)
        {
            Item = item;
            Count = count;
        }

        public bool Completed
        {
            get { return Count == CurrentCount; }
        }
    }

    public class BuildingCmp : Component
    {
        public List<RequiredResource> RequiredResources;

        public List<Tile> Tiles;

        public bool ReadyToBuild
        {
            get; private set;
        }

        public int Rows
        {
            get; private set;
        }

        public int Columns
        {
            get; private set;
        }

        public BuildingCmp(int rows, int columns) : base(false, false)
        {
            Rows = rows;
            Columns = columns;

            RequiredResources = new List<RequiredResource>();
        }

        public void AddRequiredResource(Item item, int count)
        {
            RequiredResources.Add(new RequiredResource(item, count));
        }

        public void AddTiles(List<Tile> tiles)
        {
            Tiles = tiles;
        }

        public void AddItem(Item item, int count)
        {
            ReadyToBuild = true;

            for (int i = 0; i < RequiredResources.Count; i++)
            {
                if (RequiredResources[i].Item == item)
                {
                    RequiredResources[i].CurrentCount += count;

                    Tile tile = Tiles[i];
                    tile.Item = item;
                    tile.BuildingLayerId = item.Id;
                    tile.ItemToAdd = item;
                    tile.ItemCount = tile.ItemCount + count;
                    tile.ItemToAddCount = tile.ItemCount;
                }

                if (!RequiredResources[i].Completed)
                    ReadyToBuild = false;
            }

            if(ReadyToBuild)
            {
                GamePlayState.JobList.Add(new Job(Tiles[0], JobType.BUILD));
            }
                
        }

        public override Component Clone()
        {
            BuildingCmp buildingCmp = new BuildingCmp(Rows, Columns);
            for(int i = 0; i < RequiredResources.Count; i++)
            {
                buildingCmp.AddRequiredResource(RequiredResources[i].Item, RequiredResources[i].Count);
            }
            return buildingCmp;
        }

    }
}
