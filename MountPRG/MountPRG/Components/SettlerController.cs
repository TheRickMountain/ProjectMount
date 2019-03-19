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

        private Entity cargo;
        private Tile stockpileTile;

        public SettlerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;

            Name = NameGenerator.GetInstance.GenerateMaleName();
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
                if (myJob == null && (GamePlayState.JobSystem.Count > 0))
                {
                    for (int i = 0; i < GamePlayState.JobSystem.Count; i++)
                    {
                        Job job = GamePlayState.JobSystem.Get(i);
                        if (job.JobType == JobType.HARVEST)
                        {
                            stockpileTile = GamePlayState.StockpileList.GetTileForEntity(job.Tile.Entity.Get<Gatherable>().Entity);

                            if (stockpileTile != null)
                            {
                                stockpileTile.EntityToAdd = job.Tile.Entity.Get<Gatherable>().Entity;

                                GamePlayState.JobSystem.Remove(i);
                                myJob = job;
                                SetDestTile(myJob.Tile);
                                settlerState = SettlerState.WORKING;
                                break;
                            }
                        }
                        else if(job.JobType == JobType.CHOP)
                        {
                            Tile tile = job.Tile;
                            // Если путь к одной из сторон дерева найден, то берем работу
                            if(CheckWalkablePath(tile.GetNeighbours(false)[i]))
                            {
                                GamePlayState.JobSystem.Remove(i);
                                myJob = job;
                                SetDestTile(myJob.Tile.GetNeighbours(false)[i]);
                                settlerState = SettlerState.WORKING;
                                break;
                            }
                        }
                    }
                }
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

                            cargo = myJob.Tile.Entity.Get<Gatherable>().Entity;
                            cargo.Get<Gatherable>().Count = myJob.Tile.Entity.Get<Gatherable>().Count;
                            if (!myJob.Tile.Entity.Get<Gatherable>().EntityHolder)
                                GamePlayState.TileMap.RemoveEntity(myJob.Tile.X, myJob.Tile.Y);
                            else
                                myJob.Tile.Entity.Get<Gatherable>().Count = 0;

                            SetDestTile(stockpileTile);
                        }
                    }
                    else
                    {
                        if (currTile == stockpileTile)
                        {
                            if (stockpileTile.Entity == null)
                            {
                                GamePlayState.TileMap.AddEntity(stockpileTile.X, stockpileTile.Y, cargo);
                            }
                            else
                            {
                                stockpileTile.Entity.Get<Gatherable>().Count += 1;
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

                        GamePlayState.TileMap.RemoveEntity(myJob.Tile.X, myJob.Tile.Y);
                        GamePlayState.TileMap.AddEntity(myJob.Tile.X, myJob.Tile.Y, new Wood());

                        myJob = null;

                        settlerState = SettlerState.WAITING;
                    }
                }
            }

            MovementUpdate(gameTime);
        }

        private bool CheckWalkablePath(Tile tile)
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
