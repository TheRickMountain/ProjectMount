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
        MOVE,
    }

    public enum SettlerState
    {
        WAITING,
        WORKING,
        CHECK_JOB,
    }

    public class SettlerControllerCmp : Component
    {
        public string Name
        {
            get; private set;
        }

        public Tile CurrentTile;
        private Tile nextTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 3f;

        private AnimatedSpriteCmp sprite;
        private AnimationState animationState = AnimationState.IDLE;
        public SettlerState SettlerState = SettlerState.WAITING;

        public List<Task> Tasks = new List<Task>();
        public Task CurrentTask;

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

        public bool RebuildPath = false;

        public int JobTime = 2;

        public SettlerControllerCmp(AnimatedSpriteCmp sprite)
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
            CurrentTile = nextTile = GamePlayState.TileMap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));
        }

        public override void Update(GameTime gameTime)
        {
            UpdateStats(gameTime);

            switch (SettlerState)
            {
                case SettlerState.WAITING:
                    {
                        if (MathUtils.InRange(GamePlayState.WorldTimer.TimeOfDay, 180, 359))
                        {
                            // Получаем тайл путь к закрепленной хижине
                            if (Hut != null)
                            {
                                /*Tile hutTile = Hut.Get<HutCmp>().Tiles[1];
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
                                }*/
                            }
                            else
                            {
                                SettlerState = SettlerState.WORKING;
                                Tasks.Add(new Task(TaskType.SLEEP, null, 0));
                                CurrentTask = Tasks[0];
                            }
                        }
                        else if (satiety <= 50 && GamePlayState.StockpileList.Count > 0)
                        {
                            Tile tile = GamePlayState.StockpileList.Get(stockpileCount)[stockpileTileX, stockpileTileY];

                            if (tile.Item != null && tile.Item.Consumable && IsWalkable(tile))
                            {
                                SettlerState = SettlerState.WORKING;

                                //float hunger = MAX_SATIETY - satiety;
                                //int foodUnits = (int)Math.Round(hunger / tile.Item.FoodValue);

                                Tasks.Add(new Task(TaskType.MOVE, tile, 0));
                                Tasks.Add(new Task(TaskType.EAT, tile, 5));
                                CurrentTask = Tasks[0];

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
                        if (CurrentTask != null)
                        {
                            switch (CurrentTask.TaskType)
                            {
                                case TaskType.MOVE:
                                    {
                                        if (MoveTo(CurrentTask.Tile, gameTime))
                                            NextTask();
                                    }
                                    break;
                                case TaskType.MINE:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            Tile tile = CurrentTask.Tile;
                                            tile.IsSelected = false;

                                            tile.AddItem(ItemDatabase.GetItemById(TileMap.STONE), 1);

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.CHOP:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            Tile tile = CurrentTask.Tile;
                                            tile.IsSelected = false;

                                            tile.AddItem(tile.Entity.Get<Mineable>().Item, 1);
                                            tile.RemoveEntity();

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.TAKE:
                                    {
                                        Tile tile = CurrentTask.Tile;
                                        tile.IsSelected = false;

                                        cargo = tile.Item;
                                        cargoCount = 1;

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

                                        NextTask();
                                    }
                                    break;
                                case TaskType.PUT:
                                    {
                                        Tile tile = CurrentTask.Tile;
                                        if (tile.Entity != null)
                                        {
                                            BuildingCmp building = tile.Entity.Get<BuildingCmp>();
                                            building.AddItem(cargo, cargoCount);
                                        }
                                        else
                                        {
                                            tile.Item = cargo;
                                            tile.ItemCount++;
                                            tile.ItemToAdd = cargo;
                                            tile.BuildingLayerId = cargo.Id;
                                        }

                                        cargo = null;
                                        cargoCount = 0;

                                        NextTask();
                                    }
                                    break;
                                case TaskType.HARVEST:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            Tile tile = CurrentTask.Tile;
                                            tile.IsSelected = false;
                                            Entity entity = tile.Entity;
                                            GatherableCmp gatherable = entity.Get<GatherableCmp>();
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
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            Tile tile = CurrentTask.Tile;
                                            tile.IsSelected = false;
                                            cargo = ItemDatabase.GetItemById(TileMap.FISH);
                                            cargoCount = 1;

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.EAT:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            //Tile tile = currentTask.Tile;
                                            //satiety += tile.Item.FoodValue;
                                            //tile.RemoveItem();

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.SLEEP:
                                    {
                                        if (Hut != null)
                                            Parent.Visible = false;
                                        else
                                        {
                                            sprite.CurrentAnimation = AnimationKey.Sleep;
                                            animationState = AnimationState.MOVE;
                                        }

                                        if (MathUtils.InRange(GamePlayState.WorldTimer.TimeOfDay, 0, 180))
                                        {
                                            Parent.Visible = true;
                                            sprite.CurrentAnimation = AnimationKey.Down;
                                            animationState = AnimationState.IDLE;
                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.PLOW:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            CurrentTask.Tile.GroundLayerId = TileMap.FARM_TILE;

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.PLANT:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            CurrentTask.Tile.BuildingLayerId = TileMap.WHEAT_SEED_TILE;

                                            cargo = null;
                                            cargoCount = 0;

                                            NextTask();
                                        }
                                    }
                                    break;
                                case TaskType.BUILD:
                                    {
                                        if (WorkProgress(CurrentTask.Time, gameTime))
                                        {
                                            CurrentTask.Tile.Entity.Visible = true;

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
                        myJob.CheckJob(this);
                    }
                    break;
            }

            if (animationState == AnimationState.IDLE)
                sprite.ResetAnimation();
        }

        private bool StockpileIsAvailableFor(Tile tile, Item item)
        {
            if (tile.ItemToAdd == null)
                return true;
            else if (tile.ItemToAdd == item && tile.ItemToAddCount < 10)
                return true;

            return false;
        }

        public void NextJob()
        {
            SettlerState = SettlerState.WAITING;
            myJob.JobState = JobState.AVAILABLE;
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
                    if (job.JobState == JobState.AVAILABLE)
                    {
                        job.JobState = JobState.UNAVAILABLE;
                        myJob = job;
                        SettlerState = SettlerState.CHECK_JOB;
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

                if (CurrentTask != null)
                {
                    switch (CurrentTask.TaskType)
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
            Tasks.Remove(CurrentTask);
            if (Tasks.Count > 0)
            {
                CurrentTask = Tasks[0];
            }
            else
            {
                SettlerState = SettlerState.WAITING;
                if (myJob != null)
                {
                    myJob.JobState = JobState.COMPLETED;
                    myJob = null;
                    jobCount = 0;
                }
            }
        }

        private bool MoveTo(Tile destTile, GameTime gameTime)
        {
            if (!CurrentTile.Equals(destTile) && pathAStar == null)
            {
                SetDestTile(destTile);
            }
            else
            {
                animationState = AnimationState.MOVE;

                if (CurrentTile.Equals(destTile))
                {
                    pathAStar = null;
                    animationState = AnimationState.IDLE;
                    return true;
                }
                else
                {

                    if (RebuildPath && nextTile.Equals(CurrentTile))
                    {
                        if (!destTile.Walkable)
                        {
                            Tasks.Clear();
                            CurrentTask = null;
                            myJob = null;
                            SettlerState = SettlerState.WAITING;
                            animationState = AnimationState.IDLE;
                            RebuildPath = false;
                            return false;
                        }

                        SetDestTile(destTile);
                        if (pathAStar == null)
                        {
                            Tasks.Clear();
                            CurrentTask = null;
                            myJob = null;
                            SettlerState = SettlerState.WAITING;
                            animationState = AnimationState.IDLE;
                            RebuildPath = false;
                            return false;
                        }
                        RebuildPath = false;
                    }

                    MovementUpdate(gameTime);
                }
            }

            return false;
        }

        private void SetDestTile(Tile destTile)
        {
            if (destTile.Walkable)
            {
                CurrentTile = nextTile = destTile.Tilemap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));

                pathAStar = new PathAStar(CurrentTile, destTile, destTile.Tilemap.GetTileGraph().Nodes, destTile.Tilemap);

                if (pathAStar.Length == -1)
                    pathAStar = null;
            }
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (nextTile.Equals(CurrentTile))
            {
                nextTile = pathAStar.NextTile;

                if (CurrentTile.X > nextTile.X)
                {
                    sprite.CurrentAnimation = AnimationKey.Left;
                }
                else if (CurrentTile.X < nextTile.X)
                {
                    sprite.CurrentAnimation = AnimationKey.Right;
                }

                if (CurrentTile.Y > nextTile.Y)
                {
                    sprite.CurrentAnimation = AnimationKey.Up;
                }
                else if (CurrentTile.Y < nextTile.Y)
                {
                    sprite.CurrentAnimation = AnimationKey.Down;
                }
            }


            float distToTravel = MathUtils.Distance(CurrentTile.X, CurrentTile.Y, nextTile.X, nextTile.Y);

            float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float percThisFrame = distThisFrame / distToTravel;

            movementPerc += percThisFrame;
            if (movementPerc >= 1)
            {
                CurrentTile = nextTile;
                movementPerc = 0;
            }

            Parent.X = MathUtils.Lerp(CurrentTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
            Parent.Y = MathUtils.Lerp(CurrentTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;
        }

        public bool IsWalkable(Tile destTile)
        {
            if (destTile == null)
                return false;
            else if (!destTile.Walkable)
                return false;

            PathAStar pathAStar = new PathAStar(CurrentTile, destTile, destTile.Tilemap.GetTileGraph().Nodes, destTile.Tilemap);
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (slider.Visible)
            {
                slider.Draw(spriteBatch);
            }
        }

        public override Component Clone()
        {
            throw null;
        }
    }
}
