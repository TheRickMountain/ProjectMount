using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class AIController : Component
    {

        private Tile currTile;
        private Tile nextTile;
        private Tile destTile;

        private Tile newDestTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 8f;

        public AIController() : base(true, false)
        {
            
        }

        public override void Initialize()
        {
            currTile = destTile = nextTile = GamePlayState.TileMap
                .GetTile((int)(Parent.X / TileMap.TILE_SIZE), (int)(Parent.Y / TileMap.TILE_SIZE));
            currTile.Entity = Parent;
            currTile.IsWalkable = false;
        }

        public override void Update(GameTime gameTime)
        {
            /*if (currTile == destTile)
            {
                SetDestTile(GamePlayState.TileMap.GetTile(MyRandom.Range(0, 31), MyRandom.Range(0, 31)),
                    GamePlayState.TileMap.GetTileGraph().Nodes,
                                    GamePlayState.TileMap);
            }

            MovementUpdate(gameTime);*/
        }

        private void MovementUpdate(GameTime gameTime)
        {
            if (currTile.Equals(destTile))
                pathAStar = null;

            if (pathAStar != null)
            {

                if (nextTile.Equals(currTile))
                {
                    nextTile.Entity = null;
                    nextTile.IsWalkable = true;

                    nextTile = pathAStar.NextTile;

                    nextTile.Entity = Parent;
                    nextTile.IsWalkable = false;
                }


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
            }
        }

        private void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (tile.IsWalkable)
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
