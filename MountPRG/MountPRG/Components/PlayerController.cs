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
    public enum PlayerState
    {
        IDLE,
        MOVE
    }

    public class PlayerController : Component
    {
        private Tile currTile;
        private Tile nextTile;
        private Tile destTile;

        private Tile newDestTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 3f;

        private AnimatedSprite sprite;
        private PlayerState playerState = PlayerState.IDLE;

        private Job myJob;

        private Entity cargo;
        private Tile stockpileTile;

        public PlayerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;
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
            if(myJob != null)
            {
                if(myJob.JobType == JobType.GATHER)
                {
                    if (cargo != null)
                    {
                        if(currTile == destTile)
                        {
                            if(currTile.Entity == null)
                                GamePlayState.TileMap.AddEntity(currTile.X, currTile.Y, cargo, true);
                            currTile.EntityCount += 1;

                            cargo = null;
                            myJob = null;
                        }
                    }
                    else
                    {
                        if (myJob.Tile == currTile)
                        {
                            currTile.Selected = false;

                            if (currTile.Entity.Get<Gatherable>().Item.Id == TileMap.HAY)
                            {
                                cargo = new Hay();
                                GamePlayState.TileMap.RemoveEntity(currTile.X, currTile.Y);
                            }
                            else if (currTile.Entity.Get<Gatherable>().Item.Id == TileMap.BERRY)
                            {
                                if (currTile.Entity.Tag.Equals("Bush"))
                                {
                                    currTile.Entity.Get<Gatherable>().Count -= 1;
                                    if (currTile.Entity.Get<Gatherable>().Count == 0)
                                        currTile.Entity.Get<Sprite>().Source = GamePlayState.TileSet.SourceRectangles[TileMap.BUSH];
                                }

                                cargo = new Berry();
                            }
                            else
                            {
                                cargo = currTile.Entity;
                                GamePlayState.TileMap.RemoveEntity(currTile.X, currTile.Y);
                            }

                            

                            SetDestTile(stockpileTile);
                        }
                    }
                }
            }

            if (myJob == null && (GamePlayState.JobSystem.Count > 0))
            {
                for (int i = 0; i < GamePlayState.JobSystem.Count; i++)
                {
                    Job job = GamePlayState.JobSystem.Get(i);
                    if (job.JobType == JobType.GATHER)
                    {
                        stockpileTile = GamePlayState.StockpileList.GetTileForItem(job.Tile.Entity.Get<Gatherable>().Item);
                        if (stockpileTile != null)
                        {
                            if (job.Tile.Entity.Get<Gatherable>().Item.Id == TileMap.HAY)
                            {
                                stockpileTile.EntityToAdd = new Hay();
                            }
                            else if(job.Tile.Entity.Get<Gatherable>().Item.Id == TileMap.BERRY)
                            {
                                stockpileTile.EntityToAdd = new Berry();
                            }
                            else
                            {
                                stockpileTile.EntityToAdd = job.Tile.Entity;
                            }

                            GamePlayState.JobSystem.Remove(i);
                            myJob = job;
                            myJob.Tile.Selected = true;
                            SetDestTile(myJob.Tile);
                            break;
                        }
                    }
                }
            }

            MovementUpdate(gameTime);
        }

        private void TakeEntity(Tile tile)
        {

        }

        private void PlaceEntity(Tile tile)
        {

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
                playerState = PlayerState.IDLE;
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

                playerState = PlayerState.MOVE;
            }

            if (playerState == PlayerState.IDLE)
                sprite.ResetAnimation();
        }
    }
}
