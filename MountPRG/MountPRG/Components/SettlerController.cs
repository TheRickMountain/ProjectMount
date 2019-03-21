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
        SLEEPING
    }

    public class SettlerController : Component
    {
        public string Name
        {
            get; private set;
        }

        private Tile currTile;
        private Tile nextTile;
        private Tile destTile;

        private Tile newDestTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 3f;

        private AnimatedSprite sprite;
        private AnimationState animationState = AnimationState.IDLE;
        private SettlerState settlerState = SettlerState.WAITING;

        private Job myJob;

        private Item cargo;
        private int cargoCount;

        private Tile stockpileTile;

        private Timer timer;
        private SliderUI slider;

        public SettlerController(AnimatedSprite sprite)
            : base(true, true)
        {
            this.sprite = sprite;

            Name = NameGenerator.GetInstance.GenerateMaleName();

            timer = new Timer();
            slider = new SliderUI(16, 3, Color.Black, Color.Orange);
        }

        public override void Initialize()
        {
            currTile = destTile = nextTile = GamePlayState.TileMap
                .GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));
        }

        public override void Update(GameTime gameTime)
        {
            /*if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                // Выбираем цель для отправления персонажа
                int x = GamePlayState.Camera.GetCellX();
                int y = GamePlayState.Camera.GetCellY();
                Tile tile = GamePlayState.TileMap.GetTile(x, y);
                if (tile != null && tile.IsWalkable)
                {
                    if (playerState == PlayerState.MOVE)
                    {
                        newDestTile = tile;
                    }
                    else
                    {
                        SetDestTile(tile);
                    }
                }
            }*/
            if (settlerState == SettlerState.WAITING)
            {
                FindJob();
            }
            else if (settlerState == SettlerState.WORKING)
            {
                if (myJob.JobType == JobType.HARVEST)
                {
                    if (cargo == null)
                    {
                        if (myJob.Tile == currTile)
                        {
                            myJob.Tile.Selected = false;

                            if (WorkProgress(gameTime, myJob))
                            {
                                cargo = myJob.Tile.Entity.Get<Gatherable>().Item;
                                cargoCount = myJob.Tile.Entity.Get<Gatherable>().Count;

                                if (!myJob.Tile.Entity.Get<Gatherable>().ItemHolder)
                                    GamePlayState.TileMap.RemoveEntity(myJob.Tile.X, myJob.Tile.Y);
                                else
                                    myJob.Tile.Entity.Get<Gatherable>().Count = 0;

                                SetDestTile(stockpileTile);
                            }
                        }
                    }
                    else
                    {
                        if (currTile == stockpileTile)
                        {
                            if (stockpileTile.Item == null)
                            {
                                stockpileTile.AddItem(cargo, cargoCount);
                            }
                            else
                            {
                                stockpileTile.ItemCount += cargoCount;
                            }

                            cargo = null;
                            myJob = null;

                            settlerState = SettlerState.WAITING;
                        }
                    }
                }
                else if(myJob.JobType == JobType.HAUL)
                {
                    if (cargo == null)
                    {
                        if (myJob.Tile == currTile)
                        {
                            myJob.Tile.Selected = false;

                            cargo = myJob.Tile.Item;
                            cargoCount = myJob.Tile.ItemCount;

                            myJob.Tile.RemoveItem();

                            SetDestTile(stockpileTile);
                        }
                    }
                    else
                    {
                        if (currTile == stockpileTile)
                        {
                            if (stockpileTile.Item == null)
                            {
                                stockpileTile.AddItem(cargo, cargoCount);
                            }
                            else
                            {
                                stockpileTile.ItemCount += cargoCount;
                            }

                            cargo = null;
                            myJob = null;

                            settlerState = SettlerState.WAITING;
                        }
                    }
                }
                else if(myJob.JobType == JobType.CHOP)
                {
                    if(currTile.Equals(destTile))
                    {
                        myJob.Tile.Selected = false;

                        if (WorkProgress(gameTime, myJob))
                        {
                            GamePlayState.TileMap.RemoveEntity(myJob.Tile.X, myJob.Tile.Y);
                            myJob.Tile.AddItem(ItemDatabase.GetItemById(TileMap.WOOD), 1);

                            myJob = null;

                            settlerState = SettlerState.WAITING;
                        }
                    }
                }
                else if(myJob.JobType == JobType.MINE)
                {
                    if(currTile.Equals(destTile))
                    {
                        myJob.Tile.Selected = false;

                        if (WorkProgress(gameTime, myJob))
                        {
                            myJob.Tile.BuildingLayerId = -1;
                            myJob.Tile.AddItem(ItemDatabase.GetItemById(TileMap.STONE), 1);

                            myJob = null;

                            settlerState = SettlerState.WAITING;
                        }
                    }
                }
                else if(myJob.JobType == JobType.FISH)
                {
                    if (cargo == null)
                    {
                        if (currTile.Equals(destTile))
                        {
                            myJob.Tile.Selected = false;

                            if (WorkProgress(gameTime, myJob))
                            {
                                cargo = ItemDatabase.GetItemById(TileMap.FISH);
                                cargoCount = 1;

                                SetDestTile(stockpileTile);
                            }
                        }
                    }
                    else
                    {
                        if (currTile == stockpileTile)
                        {
                            if (stockpileTile.Item == null)
                            {
                                stockpileTile.AddItem(cargo, cargoCount);
                            }
                            else
                            {
                                stockpileTile.ItemCount += cargoCount;
                            }

                            cargo = null;
                            myJob = null;

                            settlerState = SettlerState.WAITING;
                        }
                    }
                }
            }

            MovementUpdate(gameTime);
        }

        private bool WorkProgress(GameTime gameTime, Job job)
        {
            float time = timer.GetTime(gameTime);
            slider.SetValue(time, myJob.JobTime);
            slider.Visible = true;
            slider.X = Parent.X;
            slider.Y = Parent.Y - 10;

            if (time >= job.JobTime)
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

        private void FindJob()
        {
            if ((GamePlayState.JobSystem.Count > 0) && myJob == null)
            {
                for (int i = 0; i < GamePlayState.JobSystem.Count; i++)
                {
                    Job job = GamePlayState.JobSystem.Get(i);
                    if (job.JobType == JobType.HARVEST)
                    {
                        stockpileTile = GamePlayState.StockpileList.GetTileForItem(job.Tile.Entity.Get<Gatherable>().Item);

                        if (stockpileTile != null)
                        {
                            stockpileTile.ItemToAdd = job.Tile.Entity.Get<Gatherable>().Item;

                            GamePlayState.JobSystem.Remove(i);
                            myJob = job;
                            SetDestTile(myJob.Tile);
                            settlerState = SettlerState.WORKING;
                            break;
                        }
                    }
                    else if(job.JobType == JobType.FISH)
                    {
                        stockpileTile = GamePlayState.StockpileList.GetTileForItem(ItemDatabase.GetItemById(TileMap.FISH));

                        Tile tile = null;
                        // Если тайл непроходимый, то ищем путь подхода к нему
                        if (!job.Tile.Walkable)
                            tile = GetWalkablePathTo(job.Tile);
                        else
                            tile = job.Tile;

                        if (stockpileTile != null && tile != null)
                        {
                            stockpileTile.ItemToAdd = ItemDatabase.GetItemById(TileMap.FISH);

                            GamePlayState.JobSystem.Remove(i);
                            myJob = job;
                            SetDestTile(tile);
                            settlerState = SettlerState.WORKING;
                            break;
                        }
                    }
                    else if(job.JobType == JobType.HAUL)
                    {
                        stockpileTile = GamePlayState.StockpileList.GetTileForItem(job.Tile.Item);

                        if (stockpileTile != null)
                        {
                            stockpileTile.ItemToAdd = job.Tile.Item;

                            GamePlayState.JobSystem.Remove(i);
                            myJob = job;
                            SetDestTile(myJob.Tile);
                            settlerState = SettlerState.WORKING;
                            break;
                        }
                    }
                    else if (job.JobType == JobType.CHOP || job.JobType == JobType.MINE)
                    {
                        Tile tile = null;
                        // Если тайл непроходимый, то ищем путь подхода к нему
                        if (!job.Tile.Walkable)
                            tile = GetWalkablePathTo(job.Tile);
                        else
                            tile = job.Tile;

                        if (tile != null)
                        {
                            GamePlayState.JobSystem.Remove(i);
                            myJob = job;
                            SetDestTile(tile);
                            settlerState = SettlerState.WORKING;
                            break;
                        }
                    }
                }
            }
        }

        private Tile GetWalkablePathTo(Tile tile)
        {
            List<Tile> neighbours = tile.GetNeighbours(false);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (!neighbours[i].Walkable)
                    continue;
                else if (CheckPathIsWalkable(neighbours[i]))
                    return neighbours[i];
            }

            return null;
        }


        private bool CheckPathIsWalkable(Tile tile)
        {
            PathAStar pAS = new PathAStar(currTile, tile, tile.Tilemap.GetTileGraph().Nodes, tile.Tilemap);
            return pAS.Length != -1;
        }

        private void SetDestTile(Tile tile)
        {
            if (tile.Walkable)
            {
                currTile = nextTile = tile.Tilemap.GetTile((int)(Parent.X / TileMap.TILE_SIZE),
                    (int)(Parent.Y / TileMap.TILE_SIZE));
                pathAStar = new PathAStar(currTile, tile, tile.Tilemap.GetTileGraph().Nodes, tile.Tilemap);
                if (pathAStar.Length != -1)
                    destTile = tile;
                else
                    pathAStar = null;
            }
        }

        private void SetDestTile(Tile cTile, Tile dTile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (dTile.Walkable)
            {
                currTile = nextTile = cTile;
                pathAStar = new PathAStar(currTile, dTile, nodes, tilemap);
                if (pathAStar.Length != -1)
                    destTile = dTile;
                else
                    pathAStar = null;
            }
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (currTile.Equals(destTile))
            {
                pathAStar = null;
                animationState = AnimationState.IDLE;
            }

            if (pathAStar != null)
            {
                if (nextTile.Equals(currTile))
                    nextTile = pathAStar.NextTile;


                float distToTravel = MathUtils.Distance(currTile.X, currTile.Y, nextTile.X, nextTile.Y);

                float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                float percThisFrame = distThisFrame / distToTravel;

                movementPerc += percThisFrame;
                if (movementPerc >= 1)
                {
                    currTile = nextTile;

                    if (newDestTile != null)
                    {
                        SetDestTile(currTile, newDestTile,
                            GamePlayState.TileMap.GetTileGraph().Nodes,
                                GamePlayState.TileMap);
                        newDestTile = null;
                    }

                    movementPerc = 0;
                }

                Parent.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
                Parent.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;

                animationState = AnimationState.MOVING;
            }

            if (animationState == AnimationState.IDLE)
                sprite.ResetAnimation();
        }
    }
}
