using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public enum AnimationState
    {
        IDLE,
        MOVING,
    }

    public enum SettlerState
    {
        WAITING,
        WORKING,
        CHECK_JOB,
    }

    public class SettlerController : Component
    {
        public string Name
        {
            get; private set;
        }

        private Tile currTile;
        private Tile nextTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 3f; 

        private AnimatedSprite sprite;
        private AnimationState animationState = AnimationState.IDLE;
        private SettlerState settlerState = SettlerState.WAITING;

        private List<Task> tasks = new List<Task>();
        private Task currentTask;

        private Job myJob;

        private Item cargo;
        private int cargoCount;

        private Timer timer;
        private SliderUI slider;

        public Hut Hut;

        private Timer statsTimer;

        private const float MAX_SATIETY = 100f;
        private float satiety = 100.0f;

        private const float MAX_ENDURANCE = 100f;
        private float endurance = 100.0f;

        private int jobCount = 0;

        private int stockpileCount = 0;
        private int stockpileTileX = 0;
        private int stockpileTileY = 0;

        public SettlerController(AnimatedSprite sprite)
            : base(true, true)
        {
            this.sprite = sprite;

            Name = NameGenerator.GetInstance.GenerateMaleName();

            timer = new Timer();
            slider = new SliderUI(16, 3, Color.Black, Color.Orange);
            slider.Visible = false;

            statsTimer = new Timer();
        }

        public override void Initialize()
        {
            currTile = nextTile = GamePlayState.TileMap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));
        }

        public override void Update(GameTime gameTime)
        {
            UpdateStats(gameTime);

            switch(settlerState)
            {
                case SettlerState.WAITING:
                    {
                        if (MathUtils.InRange(GamePlayState.WorldTimer.TimeOfDay, 180, 359))
                        {
                            // Получаем рабочий тайл
                            if (Hut != null)
                            {
                                Tile hutTile = Hut.Get<HutCmp>().Tiles[1];
                                // Делаем его проходимым, чтобы получить доступ к его соседнему тайлу
                                hutTile.Walkable = true;
                                PathAStar pathAStar = new PathAStar(currTile, hutTile, hutTile.Tilemap.GetTileGraph().Nodes, hutTile.Tilemap);
                                hutTile.Walkable = false;
                                if (pathAStar.Length != -1)
                                {
                                    List<Tile> path = pathAStar.Path.ToList();
                                    // Означает что тайл находится прямо рядом с поселенцем
                                    if (path.Count > 1)
                                        tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                                    tasks.Add(new Task(TaskType.SLEEP, hutTile, 0));
                                    currentTask = tasks[0];

                                    settlerState = SettlerState.WORKING;
                                }
                            }
                        }
                        else if (satiety <= 50 && GamePlayState.StockpileList.Count > 0)
                        {
                            Tile tile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                            if (tile.Item != null && tile.Item.Consumable && tile.ItemToRemove == null && IsWalkable(tile))
                            {
                                tile.ItemToRemove = tile.Item;

                                settlerState = SettlerState.WORKING;

                                //float hunger = MAX_SATIETY - satiety;
                                //int foodUnits = (int)Math.Round(hunger / tile.Item.FoodValue);

                                tasks.Add(new Task(TaskType.MOVE, tile, 0));
                                tasks.Add(new Task(TaskType.EAT, tile, 5));
                                currentTask = tasks[0];

                                ResetStockpileCounter();
                            }
                            else
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
                                            // Если еды в складах нету, то поселенец продолжает работать
                                            GetNewJob();

                                            stockpileCount = 0;
                                        }
                                        stockpileTileY = 0;
                                    }
                                    stockpileTileX = 0;
                                }
                            }
                        }
                        else
                        {
                            GetNewJob();
                        }
                    }
                    break;
                case SettlerState.WORKING:
                    {
                        if (currentTask != null)
                        {
                            switch (currentTask.TaskType)
                            {
                                case TaskType.MOVE:
                                    {
                                        if (MoveTo(currentTask.Tile, gameTime))
                                            NextTask();
                                    }
                                    break;
                                case TaskType.PROCESS:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            Tile tile = currentTask.Tile;
                                            tile.Selected = false;
                                            if (tile.BuildingLayerId == TileMap.STONE_1_BLOCK || tile.BuildingLayerId == TileMap.STONE_2_BLOCK)
                                            {
                                                tile.AddItem(ItemDatabase.GetItemById(TileMap.STONE), 1);
                                            }
                                            else if (tile.Entity != null)
                                            {
                                                Mineable mineable = tile.Entity.Get<Mineable>();
                                                if (mineable != null)
                                                {
                                                    tile.RemoveEntity();
                                                    tile.AddItem(mineable.Item, 1);
                                                }
                                            }

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.TAKE:
                                    {
                                        Tile tile = currentTask.Tile;
                                        tile.Selected = false;
                                        cargo = tile.Item;
                                        cargoCount = tile.ItemCount;
                                        tile.RemoveItem();

                                        NextTask();
                                    }
                                    break;
                                case TaskType.PUT:
                                    {
                                        Tile tile = currentTask.Tile;
                                        tile.AddItem(cargo, tile.ItemCount + cargoCount);

                                        cargo = null;
                                        cargoCount = 0;

                                        NextTask();
                                    }
                                    break;
                                case TaskType.HARVEST:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            Tile tile = currentTask.Tile;
                                            tile.Selected = false;
                                            Entity entity = tile.Entity;
                                            Gatherable gatherable = entity.Get<Gatherable>();
                                            cargo = gatherable.Item;
                                            cargoCount = gatherable.Count;

                                            if (gatherable.ItemHolder)
                                            {
                                                gatherable.Count -= 1;
                                            }
                                            else
                                            {
                                                tile.RemoveEntity();
                                            }

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.FISH:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            Tile tile = currentTask.Tile;
                                            tile.Selected = false;
                                            cargo = ItemDatabase.GetItemById(TileMap.FISH);
                                            cargoCount = 1;

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.EAT:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            Tile tile = currentTask.Tile;
                                            satiety += tile.Item.FoodValue;
                                            tile.RemoveItem();

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.SLEEP:
                                    {
                                        if (Hut != null)
                                            Parent.Visible = false;

                                        if(MathUtils.InRange(GamePlayState.WorldTimer.TimeOfDay, 0, 180))
                                        {
                                            Parent.Visible = true;
                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.PLOW:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            currentTask.Tile.GroundLayerId = TileMap.FARM_TILE;

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.PLANT:
                                    {
                                        if (WorkProgress(currentTask.Time, gameTime))
                                        {
                                            currentTask.Tile.BuildingLayerId = TileMap.WHEAT_SEED_TILE;
                                            
                                            cargo = null;
                                            cargoCount = 0;

                                            NextTask();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case SettlerState.CHECK_JOB:
                    {
                        switch(myJob.JobType)
                        {
                            case JobType.MINE:
                            case JobType.CHOP:
                                {
                                    // Получаем рабочий тайл
                                    Tile jobTile = myJob.Tile;
                                    // Делаем его проходимым, чтобы получить доступ к его соседнему тайлу
                                    jobTile.Walkable = true;
                                    PathAStar pathAStar = new PathAStar(currTile, jobTile, jobTile.Tilemap.GetTileGraph().Nodes, jobTile.Tilemap);
                                    jobTile.Walkable = false;
                                    if (pathAStar.Length != -1)
                                    {
                                        List<Tile> path = pathAStar.Path.ToList();
                                        // Означает что тайл находится прямо рядом с поселенцем
                                        if (path.Count > 1)
                                            tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                                        tasks.Add(new Task(TaskType.PROCESS, jobTile, myJob.JobTime));
                                        currentTask = tasks[0];

                                        settlerState = SettlerState.WORKING;
                                    }
                                    else
                                    {
                                        NextJob();
                                    }
                                }
                                break;
                            case JobType.HAUL:
                                {
                                    // Получаем рабочий тайл
                                    Tile jobTile = myJob.Tile;

                                    if (IsWalkable(jobTile) && GamePlayState.StockpileList.Count > 0)
                                    {
                                        Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                                        if (stockpileTile.ItemToAdd == null && IsWalkable(stockpileTile))
                                        {
                                            // Тайл получает информацию о том какой предмет туда нужно добавить и сколько
                                            stockpileTile.ItemToAdd = jobTile.Item;

                                            // делаем работу текущей для данного поселенца
                                            settlerState = SettlerState.WORKING;

                                            tasks.Add(new Task(TaskType.MOVE, jobTile, 0));
                                            tasks.Add(new Task(TaskType.TAKE, jobTile, myJob.JobTime));
                                            tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                                            tasks.Add(new Task(TaskType.PUT, stockpileTile, 0));
                                            currentTask = tasks[0];

                                            ResetStockpileCounter();
                                        }
                                        else
                                        {
                                            stockpileTileX++;
                                            if(stockpileTileX == GamePlayState.StockpileList.Get(stockpileCount).GetLength(0))
                                            {
                                                stockpileTileY++;

                                                if (stockpileTileY == GamePlayState.StockpileList.Get(stockpileCount).GetLength(1))
                                                {
                                                    stockpileCount++;
                                                    if(stockpileCount == GamePlayState.StockpileList.Count)
                                                    {
                                                        NextJob();

                                                        stockpileCount = 0;
                                                    }
                                                    stockpileTileY = 0;
                                                }

                                                stockpileTileX = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NextJob();

                                        ResetStockpileCounter();
                                    }
                                }
                                break;
                            case JobType.HARVEST:
                                {
                                    Tile jobTile = myJob.Tile;

                                    if (GamePlayState.StockpileList.Count > 0 && IsWalkable(jobTile))
                                    {
                                        Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                                        if (stockpileTile.ItemToAdd == null && IsWalkable(stockpileTile))
                                        {
                                            stockpileTile.ItemToAdd = jobTile.Entity.Get<Gatherable>().Item;

                                            settlerState = SettlerState.WORKING;

                                            tasks.Add(new Task(TaskType.MOVE, jobTile, 0));
                                            tasks.Add(new Task(TaskType.HARVEST, jobTile, myJob.JobTime));
                                            tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                                            tasks.Add(new Task(TaskType.PUT, stockpileTile, 0));
                                            currentTask = tasks[0];

                                            ResetStockpileCounter();
                                        }
                                        else
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
                                                        NextJob();

                                                        stockpileCount = 0;
                                                    }
                                                    stockpileTileY = 0;
                                                }

                                                stockpileTileX = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NextJob();

                                        ResetStockpileCounter();
                                    }
                                }
                                break;
                            case JobType.FISH:
                                {
                                    // Получаем рабочий тайл
                                    Tile jobTile = myJob.Tile;

                                    jobTile.Walkable = true;
                                    PathAStar pathAStar = new PathAStar(currTile, jobTile, jobTile.Tilemap.GetTileGraph().Nodes, jobTile.Tilemap);
                                    jobTile.Walkable = false;
                                    if (pathAStar.Length != -1 && GamePlayState.StockpileList.Count > 0)
                                    {

                                        Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                                        if (stockpileTile.ItemToAdd == null && IsWalkable(stockpileTile))
                                        {
                                            stockpileTile.ItemToAdd = ItemDatabase.GetItemById(TileMap.FISH);

                                            settlerState = SettlerState.WORKING;

                                            List<Tile> path = pathAStar.Path.ToList();

                                            if (path.Count > 1)
                                                tasks.Add(new Task(TaskType.MOVE, path[path.Count - 2], 0));

                                            tasks.Add(new Task(TaskType.FISH, jobTile, myJob.JobTime));
                                            tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                                            tasks.Add(new Task(TaskType.PUT, stockpileTile, 0));
                                            currentTask = tasks[0];

                                            ResetStockpileCounter();
                                        }
                                        else
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
                                                        NextJob();

                                                        stockpileCount = 0;
                                                    }
                                                    stockpileTileY = 0;
                                                }

                                                stockpileTileX = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NextJob();

                                        ResetStockpileCounter();
                                    }
                                }
                                break;
                            case JobType.PLANT:
                                {
                                    Tile jobTile = myJob.Tile;

                                    // Тайл не засеян
                                    if(jobTile.BuildingLayerId == -1 && IsWalkable(jobTile))
                                    {
                                        // На складе есть необходимый предмет
                                        if (GamePlayState.StockpileList.HasItem(ItemDatabase.GetItemById(TileMap.WHEAT_SEED)))
                                        {
                                            Tile stockpileTile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                                            if (stockpileTile.Item == ItemDatabase.GetItemById(TileMap.WHEAT_SEED) && IsWalkable(stockpileTile))
                                            {
                                                settlerState = SettlerState.WORKING;

                                                tasks.Add(new Task(TaskType.MOVE, stockpileTile, 0));
                                                tasks.Add(new Task(TaskType.TAKE, stockpileTile, 0));
                                                tasks.Add(new Task(TaskType.MOVE, jobTile, 0));
                                                tasks.Add(new Task(TaskType.PLOW, jobTile, 2));
                                                tasks.Add(new Task(TaskType.PLANT, jobTile, 2));
                                                currentTask = tasks[0];

                                                ResetStockpileCounter();
                                            }
                                            else
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
                                                            NextJob();

                                                            stockpileCount = 0;
                                                        }
                                                        stockpileTileY = 0;
                                                    }

                                                    stockpileTileX = 0;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            NextJob();
                                        }
                                    }
                                    else
                                    {
                                        NextJob();
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            if (animationState == AnimationState.IDLE)
                sprite.ResetAnimation();
        }

        private void NextJob()
        {
            settlerState = SettlerState.WAITING;
            myJob.Owner = null;
            myJob = null;
            jobCount++;
        }

        private void GetNewJob()
        {
            if (myJob == null)
            {
                if (GamePlayState.JobList.Count != 0)
                {
                    if (jobCount >= GamePlayState.JobList.Count)
                        jobCount = 0;

                    Job job = GamePlayState.JobList.Get(jobCount);
                    if (job.Owner == null)
                    {
                        job.Owner = (Settler)Parent;
                        myJob = job;
                        settlerState = SettlerState.CHECK_JOB;
                    }
                    else
                    {
                        jobCount++;
                    }
                }
                else
                {
                    jobCount = 0;
                }
            }
        }

        private void ResetStockpileCounter()
        {
            stockpileCount = 0;
            stockpileTileX = 0;
            stockpileTileY = 0;
        }

        private void UpdateStats(GameTime gameTime)
        {
            float time = statsTimer.GetTime(gameTime);
            if (time > 1.0f)
            {
                endurance -= 0.1f;
                satiety -= 0.1f;
                statsTimer.Reset();

                if(currentTask != null)
                {
                    switch(currentTask.TaskType)
                    {
                        case TaskType.EAT:
                            satiety += 0.05f;
                            break;
                        case TaskType.SLEEP:
                            endurance = Math.Min(endurance + 0.6f, MAX_ENDURANCE);
                            break;
                    }
                }
            }

        }

        private void NextTask()
        {
            tasks.Remove(currentTask);
            if (tasks.Count > 0)
            {
                currentTask = tasks[0];
            }
            else
            {
                settlerState = SettlerState.WAITING;
                if (myJob != null)
                {
                    GamePlayState.JobList.Remove(myJob);
                    myJob = null;
                    jobCount = 0;
                }
            }
        }

        private bool MoveTo(Tile destTile, GameTime gameTime)
        {
            if (!currTile.Equals(destTile) && pathAStar == null)
            {
                SetDestTile(destTile);
            }
            else
            {
                animationState = AnimationState.MOVING;

                if (currTile.Equals(destTile))
                {
                    pathAStar = null;
                    animationState = AnimationState.IDLE;
                    return true;
                } else
                {
                    MovementUpdate(gameTime);
                }
            }

            return false;
        }

        private void SetDestTile(Tile destTile)
        {
            if (destTile.Walkable)
            {
                currTile = nextTile = destTile.Tilemap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));

                pathAStar = new PathAStar(currTile, destTile, destTile.Tilemap.GetTileGraph().Nodes, destTile.Tilemap);

                if (pathAStar.Length == -1)
                    pathAStar = null;
            }
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (nextTile.Equals(currTile))
            {
                nextTile = pathAStar.NextTile;

                if ((currTile.X - nextTile.X) == 1)
                {
                    sprite.CurrentAnimation = AnimationKey.Left;
                }
                else if ((currTile.X - nextTile.X) == -1)
                {
                    sprite.CurrentAnimation = AnimationKey.Right;
                }

                if ((currTile.Y - nextTile.Y) == 1)
                {
                    sprite.CurrentAnimation = AnimationKey.Up;
                }
                else if ((currTile.Y - nextTile.Y) == -1)
                {
                    sprite.CurrentAnimation = AnimationKey.Down;
                }
            }


            float distToTravel = MathUtils.Distance(currTile.X, currTile.Y, nextTile.X, nextTile.Y);

            float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float percThisFrame = distThisFrame / distToTravel;

            movementPerc += percThisFrame;
            if (movementPerc >= 1)
            {
                currTile = nextTile;
                movementPerc = 0;
            }

            Parent.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
            Parent.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;

            
        }

        private bool IsWalkable(Tile destTile)
        {
            if (destTile == null)
                return false;
            else if (!destTile.Walkable)
                return false;

            PathAStar pathAStar = new PathAStar(currTile, destTile, destTile.Tilemap.GetTileGraph().Nodes, destTile.Tilemap);
            return pathAStar.Length != -1;
        }

        private bool WorkProgress(float taskTime, GameTime gameTime)
        {
            float time = timer.GetTime(gameTime);
            slider.SetValue(time, taskTime);
            slider.Visible = true;
            slider.X = Parent.X;
            slider.Y = Parent.Y - 10;

            if (time >= taskTime)
            {
                timer.Reset();
                slider.Reset();
                slider.Visible = false;
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (slider.Visible)
            {
                slider.Draw(spriteBatch);
            }
        }
        
    }
}
