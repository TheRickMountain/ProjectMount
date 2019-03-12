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
        private float speed = 2f;

        private AnimatedSprite sprite;
        private PlayerState playerState = PlayerState.IDLE;

        private Job myJob;

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
                        SetDestTile(tile,
                                GamePlayState.TileMap.GetTileGraph().Nodes,
                                GamePlayState.TileMap);
                    }
                }
            }*/
            if(myJob == null && (GamePlayState.JobSystem.Count > 0))
            {
                myJob = GamePlayState.JobSystem.Dequeue();

                // TODO get nodes and tilemap from tile
                Tile tile = myJob.Tile;
                SetDestTile(tile, GamePlayState.TileMap.GetTileGraph().Nodes, GamePlayState.TileMap);
            }

            MovementUpdate(gameTime);
        }

        private void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (tile.Walkable)
            {
                currTile = nextTile = tilemap.GetTile((int)(Parent.X / TileMap.TILE_SIZE),
                    (int)(Parent.Y / TileMap.TILE_SIZE));
                pathAStar = new PathAStar(currTile, tile, nodes, tilemap);
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
