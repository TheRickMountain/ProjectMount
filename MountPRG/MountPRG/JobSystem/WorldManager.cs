using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class WorldManager
    {

        private Camera camera;

        private Texture2D selectorTexture;
        private Rectangle selectorDest;
        private Color selectorColor;

        private Tile firstSelectedTile;
        private Tile lastSelectedTile;

        public Tile SelectedTile
        {
            get; private set;
        }

        public JobType CurrentJobType
        {
            get; set;
        }

        public WorldManager(Camera camera)
        {
            this.camera = camera;

            selectorTexture = ResourceBank.Sprites["selector"];
            selectorDest = new Rectangle(0, 0, selectorTexture.Width, selectorTexture.Height);
            selectorColor = Color.White;

            CurrentJobType = JobType.NONE;
        }

        public void Update(GameTime gameTime)
        {
            selectorDest.X = camera.GetCellX() * TileMap.TILE_SIZE;
            selectorDest.Y = camera.GetCellY() * TileMap.TILE_SIZE;
            SelectedTile = GamePlayState.TileMap.GetTile(camera.GetCellX(), camera.GetCellY());

            if (!GUIManager.MouseOnUI)
            {
                switch (CurrentJobType)
                {
                    case JobType.NONE:
                        {
                            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                            {
                                switch(SelectedTile.Area.AreaType)
                                {
                                    case AreaType.STOCKPILE:
                                        //TODO: make it on gui manenger
                                        GUIManager.FarmUI.Close();
                                        GUIManager.HutUI.Close();
                                        GUIManager.StockpileUI.Close();

                                        GUIManager.StockpileUI.Open(GamePlayState.StockpileList.Get(SelectedTile.Area.Num));
                                        break;
                                    case AreaType.FARM:
                                        GUIManager.FarmUI.Close();
                                        GUIManager.HutUI.Close();
                                        GUIManager.StockpileUI.Close();

                                        GUIManager.FarmUI.Open(SelectedTile.Area.Num);
                                        break;
                                    case AreaType.NONE:
                                        if (SelectedTile.Entity != null)
                                        {
                                            HutCmp hut = SelectedTile.Entity.Get<HutCmp>();
                                            if (hut != null)
                                            {
                                                GUIManager.HutUI.Close();
                                                GUIManager.StockpileUI.Close();

                                                GUIManager.HutUI.Open(hut, GamePlayState.Settlers);
                                            }
                                        }
                                        break;
                                }   
                            }
                        }
                        break;
                    case JobType.HARVEST:
                        MakeHarvestJob();
                        break;
                    case JobType.CHOP:
                        MakeChopJob();
                        break;
                    case JobType.MINE:
                        MakeMineJob();
                        break;
                    case JobType.HAUL:
                        MakeHaulJob();
                        break;
                    case JobType.STOCKPILE:
                        MakeStockpile();
                        break;
                    case JobType.FISH:
                        MakeFishJob();
                        break;
                    case JobType.BUILD:
                        //MakeBuilding();
                        break;
                    case JobType.PLANT:
                        MakePlantJob();
                        break;
                }
            }
        }

        public void SetJobType(JobType jobType)
        {
            CurrentJobType = jobType;
            Console.WriteLine(CurrentJobType.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectorTexture, selectorDest, selectorColor);
        }

        private void MakeStockpile()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                firstSelectedTile = lastSelectedTile = SelectedTile;
            }

            if (firstSelectedTile != null)
            {
                if (InputManager.GetMouseButton(MouseInput.LeftButton))
                {
                    SelectArea(firstSelectedTile, lastSelectedTile, Color.White);

                    int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                    int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                    if (width < 1 || width > 5 || height < 1 || height > 5)
                    {
                        SelectArea(firstSelectedTile, SelectedTile, Color.IndianRed);
                    }
                    else
                    {
                        SelectArea(firstSelectedTile, SelectedTile, Color.LightGreen);
                    }

                    lastSelectedTile = SelectedTile;
                }

                if (InputManager.GetMouseButtonReleased(MouseInput.LeftButton))
                {
                    int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                    int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                    if (width >= 1 && width <= 5 && height >= 1 && height <= 5)
                    {
                        GamePlayState.StockpileList.Add(GetAreaTiles(firstSelectedTile, SelectedTile));
                    }

                    SelectArea(firstSelectedTile, SelectedTile, Color.White);

                    firstSelectedTile = null;
                    lastSelectedTile = null;
                }
            }
        }

        private void SelectArea(Tile firstTile, Tile lastTile, Color color)
        {
            int firstX = firstTile.X;
            int firstY = firstTile.Y;

            int lastX = lastTile.X;
            int lastY = lastTile.Y;

            if (firstX > lastX)
                MathUtils.Replace(ref firstX, ref lastX);

            if (firstY > lastY)
                MathUtils.Replace(ref firstY, ref lastY);

            for (int x = firstX; x <= lastX; x++)
            {
                for (int y = firstY; y <= lastY; y++)
                {
                    GamePlayState.TileMap.GetTile(x, y).GroundLayerColor = color;
                }
            }
        }

        private Tile[,] GetAreaTiles(Tile firstTile, Tile lastTile)
        {
            int firstX = firstTile.X;
            int firstY = firstTile.Y;

            int lastX = lastTile.X;
            int lastY = lastTile.Y;

            Tile[,] tiles = new Tile[Math.Abs(firstX - lastX) + 1, Math.Abs(firstY - lastY) + 1];

            if (firstX > lastX)
                MathUtils.Replace(ref firstX, ref lastX);

            if (firstY > lastY)
                MathUtils.Replace(ref firstY, ref lastY);

            for (int x = firstX; x <= lastX; x++)
            {
                for (int y = firstY; y <= lastY; y++)
                {
                    Tile tile = GamePlayState.TileMap.GetTile(x, y);
                    if (tile.Item != null)
                        tile.ItemToAdd = tile.Item;
                    tile.GroundLayerId = TileMap.DIRT_TILE;
                    tiles[x - firstX, y - firstY] = tile;
                }
            }

            return tiles;
        }

        private void MakeHarvestJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    if (SelectedTile.Entity != null)
                    {
                        Gatherable gatherable = SelectedTile.Entity.Get<Gatherable>();
                        if (gatherable != null && gatherable.Count > 0)
                        {
                            SelectedTile.Selected = true;
                            GamePlayState.JobList.Add(new Job(SelectedTile, JobType.HARVEST, 1));
                        }
                    }
                }
            }
        }

        private void MakeFishJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    if (SelectedTile.GroundLayerId == TileMap.WATER_1_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_2_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_3_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_4_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_FRONT_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_LEFT_TILE
                        || SelectedTile.GroundLayerId == TileMap.WATER_RIGHT_TILE)
                    {
                        SelectedTile.Selected = true;
                        GamePlayState.JobList.Add(new Job(SelectedTile, JobType.FISH, 5));
                    }
                }
            }
        }

        private void MakeHaulJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    if (SelectedTile.Item != null)
                    {
                        SelectedTile.Selected = true;
                        GamePlayState.JobList.Add(new Job(SelectedTile, JobType.HAUL));
                    }
                }
            }
        }

        private void MakeChopJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    if (SelectedTile.Entity != null)
                    {
                        Mineable mineable = SelectedTile.Entity.Get<Mineable>();
                        if (mineable != null)
                        {
                            SelectedTile.Selected = true;
                            GamePlayState.JobList.Add(new Job(SelectedTile, JobType.CHOP, 1));
                        }
                    }
                }
            }
        }

        private void MakeMineJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!SelectedTile.Selected)
                {
                    if (SelectedTile.BuildingLayerId == TileMap.STONE_1_BLOCK || SelectedTile.BuildingLayerId == TileMap.STONE_2_BLOCK)
                    {
                        SelectedTile.Selected = true;
                        GamePlayState.JobList.Add(new Job(SelectedTile, JobType.MINE, 2));
                    }
                }
            }
        }

        //private void MakeBuilding()
        //{
        //    Entity entity = GUIManager.ActionPanelUI.CurrentBuilding;
        //    entity.X = camera.GetCellX() * TileMap.TILE_SIZE;
        //    entity.Y = camera.GetCellY() * TileMap.TILE_SIZE;

        //    Building building = entity.Get<Building>();
        //    Sprite sprite = entity.Get<Sprite>();

        //    bool validToBuild = true;

        //    for (int i = camera.GetCellX(); i < camera.GetCellX() + building.Columns; i++)
        //    {
        //        for (int j = camera.GetCellY(); j < camera.GetCellY() + building.Rows; j++)
        //        {
        //            Tile tile = GamePlayState.TileMap.GetTile(i, j);
        //            if (tile == null || tile.BuildingLayerId != -1 || tile.Entity != null)
        //            {
        //                validToBuild = false;
        //                break;
        //            }
        //        }
        //    }

        //    sprite.Color = (validToBuild ? Color.Green : Color.Red) * 0.5f;

        //    if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
        //    {
        //        if (validToBuild)
        //        {
        //            List<Tile> tiles = new List<Tile>();
        //            for (int i = camera.GetCellX(); i < camera.GetCellX() + building.Columns; i++)
        //            {
        //                for (int j = camera.GetCellY(); j < camera.GetCellY() + building.Rows; j++)
        //                {
        //                    Tile tile = GamePlayState.TileMap.GetTile(i, j);
        //                    tile.Entity = GUIManager.ActionPanelUI.CurrentBuilding;
        //                    tile.Walkable = false;
        //                    tiles.Add(tile);
        //                }
        //            }

        //            if (GUIManager.ActionPanelUI.CurrentBuilding is Hut)
        //                GUIManager.ActionPanelUI.CurrentBuilding.Get<HutCmp>().AddTiles(tiles);

        //            sprite.Color = Color.White;
        //            GUIManager.ActionPanelUI.CurrentBuilding = null;
        //            GUIManager.ActionPanelUI.CurrentJobType = JobType.NONE;
        //        }
        //    }
        //}

        private void MakePlantJob()
        {
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                firstSelectedTile = lastSelectedTile = SelectedTile;
            }

            if (firstSelectedTile != null)
            {
                if (InputManager.GetMouseButton(MouseInput.LeftButton))
                {
                    SelectArea(firstSelectedTile, lastSelectedTile, Color.White);

                    int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                    int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                    if (width < 1 || width > 5 || height < 1 || height > 5)
                    {
                        SelectArea(firstSelectedTile, SelectedTile, Color.IndianRed);
                    }
                    else
                    {
                        SelectArea(firstSelectedTile, SelectedTile, Color.Turquoise);
                    }

                    lastSelectedTile = SelectedTile;
                }

                if (InputManager.GetMouseButtonReleased(MouseInput.LeftButton))
                {
                    int width = Math.Abs(firstSelectedTile.X - SelectedTile.X);
                    int height = Math.Abs(firstSelectedTile.Y - SelectedTile.Y);

                    if (width >= 1 && width <= 5 && height >= 1 && height <= 5)
                    {

                        GamePlayState.FarmList.Add(GetAreaTiles(firstSelectedTile, SelectedTile));
                    }

                    SelectArea(firstSelectedTile, SelectedTile, Color.White);

                    firstSelectedTile = null;
                    lastSelectedTile = null;
                }
            }
        }
    }
}
