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
        public Tile NextTile;

        public PathAStar PathAStar;

        private float movementPerc;
        private float speed = 6f;

        public AnimatedSpriteCmp Sprite;
        public AnimationState AnimationState = AnimationState.IDLE;
        public SettlerState SettlerState = SettlerState.WAITING;

        public Job MyJob;

        public Item Cargo;
        public int CargoCount;

        private Timer timer;
        private SliderUI slider;

        public Hut Hut;

        private Timer statsTimer;

        public float MaxSatiety = 100f;
        public float Satiety = 100.0f;

        public float MaxEndurance = 100f;
        public float Endurance = 100.0f;

        private int jobCount = 0;

        public bool RebuildPath = false;

        public int JobTime = 2;

        public SettlerControllerCmp(AnimatedSpriteCmp sprite)
        {
            this.Sprite = sprite;

            Name = NameGenerator.GetInstance.GenerateMaleName();

            timer = new Timer();
            slider = new SliderUI(16, 3, Color.Black, Color.Orange);
            slider.Visible = false;

            statsTimer = new Timer();
        }

        public override void Initialize()
        {
            CurrentTile = NextTile = GamePlayState.TileMap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));
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
                            MyJob = new SleepJob();
                            SettlerState = SettlerState.CHECK_JOB;
                        }
                            

                            /*if (MathUtils.InRange(GamePlayState.WorldTimer.TimeOfDay, 180, 359))
                            {
                                // Получаем тайл путь к закрепленной хижине
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
                                else
                                {
                                    //SettlerState = SettlerState.WORKING;
                                    //Tasks.Add(new Task(TaskType.SLEEP, null, 0));
                                    //CurrentTask = Tasks[0];
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

                                    //Tasks.Add(new Task(TaskType.MOVE, tile, 0));
                                    //Tasks.Add(new Task(TaskType.EAT, tile, 5));
                                    //CurrentTask = Tasks[0];

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
                            else*/
                            //{
                            AnimationState = AnimationState.IDLE;
                        GetNewJob();
                        //}
                    }
                    break;
                case SettlerState.WORKING:
                    {
                        MyJob.DoJob(this, gameTime);
                        /*if (myJob.CurrentTask != null)
                        {
                            switch (myJob.CurrentTask.TaskType)
                            {
                                case TaskType.EAT:
                                    {
                                        if (WorkProgress(myJob.CurrentTask.Time, gameTime))
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
                                }
                            }
                        }*/
                    }
                    break;
                case SettlerState.CHECK_JOB:
                    {
                        MyJob.CheckJob(this);
                    }
                    break;
            }

            if (AnimationState == AnimationState.IDLE)
                Sprite.ResetAnimation();
        }


        public void NextJob()
        {
            SettlerState = SettlerState.WAITING;
            MyJob.JobState = JobState.AVAILABLE;
            MyJob = null;
            jobCount++;
        }

        private void GetNewJob()
        {
            if (MyJob == null)
            {
                if (GamePlayState.JobList.Count != 0)
                {
                    if (jobCount >= GamePlayState.JobList.Count)
                        jobCount = 0;

                    Job job = GamePlayState.JobList.Get(jobCount);
                    if (job.JobState == JobState.AVAILABLE)
                    {
                        job.JobState = JobState.UNAVAILABLE;
                        MyJob = job;
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

        private void UpdateStats(GameTime gameTime)
        {
            float time = statsTimer.GetTime(gameTime);
            if (time > 1.0f)
            {
                Endurance -= 1f;
                Satiety -= 1f;
                statsTimer.Reset();
            }
        }

        private void NextTask()
        {
            MyJob.Tasks.Remove(MyJob.CurrentTask);
            if (MyJob.Tasks.Count > 0)
            {
                MyJob.CurrentTask = MyJob.Tasks[0];
            }
            else
            {
                SettlerState = SettlerState.WAITING;
                MyJob.JobState = JobState.COMPLETED;
                MyJob = null;
                jobCount = 0;
            }
        }

        public bool MoveTo(Tile destTile, GameTime gameTime)
        {
            if (!CurrentTile.Equals(destTile) && PathAStar == null)
            {
                SetDestTile(destTile);
            }
            else
            {
                AnimationState = AnimationState.MOVE;

                if (CurrentTile.Equals(destTile))
                {
                    PathAStar = null;
                    AnimationState = AnimationState.IDLE;
                    return true;
                }
                else
                {
                    MovementUpdate(gameTime);
                }
            }

            return false;
        }

        public void SetDestTile(Tile destTile)
        {
            if (destTile.Walkable)
            {
                CurrentTile = NextTile = destTile.Tilemap.GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));

                PathAStar = new PathAStar(CurrentTile, destTile, destTile.Tilemap.GetTileGraph().Nodes, destTile.Tilemap);

                if (PathAStar.Length == -1)
                    PathAStar = null;
            }
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (NextTile.Equals(CurrentTile))
            {
                // Был поставлени/убран блок, необходимо перестроить путь
                NextTile = PathAStar.NextTile;

                if (CurrentTile.X > NextTile.X)
                {
                    Sprite.CurrentAnimation = AnimationKey.Left;
                }
                else if (CurrentTile.X < NextTile.X)
                {
                    Sprite.CurrentAnimation = AnimationKey.Right;
                }

                if (CurrentTile.Y > NextTile.Y)
                {
                    Sprite.CurrentAnimation = AnimationKey.Up;
                }
                else if (CurrentTile.Y < NextTile.Y)
                {
                    Sprite.CurrentAnimation = AnimationKey.Down;
                }
            }


            float distToTravel = MathUtils.Distance(CurrentTile.X, CurrentTile.Y, NextTile.X, NextTile.Y);

            float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float percThisFrame = distThisFrame / distToTravel;

            movementPerc += percThisFrame;
            if (movementPerc >= 1)
            {
                CurrentTile = NextTile;
                movementPerc = 0;
            }

            Parent.X = MathUtils.Lerp(CurrentTile.X, NextTile.X, movementPerc) * TileMap.TILE_SIZE;
            Parent.Y = MathUtils.Lerp(CurrentTile.Y, NextTile.Y, movementPerc) * TileMap.TILE_SIZE;
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

        public bool WorkProgress(float taskTime, GameTime gameTime)
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

        public override Component Clone()
        {
            throw null;
        }
    }
}
