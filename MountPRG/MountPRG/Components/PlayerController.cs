using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public enum State
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
        private float speed = 4f;

        private AnimatedSprite sprite;
        private State state = State.IDLE;

        private bool inInventory;

        public PlayerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;
        }

        public override void Initialize()
        {
            currTile = destTile = nextTile = GamePlayState.TileMap
                .GetTile((int)(Entity.X / TileMap.TILE_SIZE), (int)(Entity.Y / TileMap.TILE_SIZE));
        }

        public override void Update(GameTime gameTime)
        {
            // Пробуем открыть сундук если игрок рядом с ним
            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                int x = GamePlayState.Camera.GetCellX();
                int y = GamePlayState.Camera.GetCellY();
                // Если игрок рядом с сундуком в одном из 4-х направлений
                if(currTile.X == x - 1 || currTile.X == x + 1
                    || currTile.Y == y - 1 || currTile.Y == y + 1)
                {
                    Tile tile = GamePlayState.TileMap.GetTile(x, y);
                    if (tile != null)
                    {
                        Entity entity = tile.Entity;
                        if (entity != null)
                        {
                            if (entity.Get<Storage>() != null)
                            {
                                GUIManager.OpenStorage(entity.Get<Storage>());
                                inInventory = true;
                            }
                        }
                    }
                }   
            }

            if (InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                if (!inInventory)
                {
                    int x = GamePlayState.Camera.GetCellX();
                    int y = GamePlayState.Camera.GetCellY();
                    Tile tile = GamePlayState.TileMap.GetTile(x, y);
                    if (tile != null && tile.IsWalkable)
                    {
                        if (state == State.MOVE)
                        {
                            newDestTile = tile;
                        }
                        else
                        {
                            SetDestTile(tile,
                                    GamePlayState.TileMap.GetTileGraph().Nodes,
                                    GamePlayState.TileMap);
                        }
                    }
                } else
                {
                    inInventory = false;
                    GUIManager.CloseStoarge();
                }
            }

            if (currTile.Equals(destTile))
            {
                pathAStar = null;
                state = State.IDLE;
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
                    
                    if(newDestTile != null)
                    {
                        SetDestTile(currTile, newDestTile,
                            GamePlayState.TileMap.GetTileGraph().Nodes,
                                GamePlayState.TileMap);
                        newDestTile = null;
                    }

                    movementPerc = 0;
                }

                Entity.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
                Entity.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;

                state = State.MOVE;
            }

            if (state == State.IDLE)
                sprite.ResetAnimation();
        }

        public void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (tile.IsWalkable)
            {
                currTile = nextTile = tilemap.GetTile((int)(Entity.X / TileMap.TILE_SIZE), 
                    (int)(Entity.Y / TileMap.TILE_SIZE));
                pathAStar = new PathAStar(currTile, tile, nodes, tilemap);
                if (pathAStar.Length != -1)
                    destTile = tile;
                else
                    pathAStar = null;
            }
        }

        public void SetDestTile(Tile cTile, Tile dTile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (dTile.IsWalkable)
            {
                currTile = nextTile = cTile;
                pathAStar = new PathAStar(currTile, dTile, nodes, tilemap);
                if (pathAStar.Length != -1)
                    destTile = dTile;
                else
                    pathAStar = null;
            }
        }
    }
}
