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

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 4f;

        private AnimatedSprite sprite;
        private State state = State.IDLE;

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
            if (state == State.IDLE)
                sprite.ResetAnimation();

            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                int x = GamePlayState.Camera.GetCellX();
                int y = GamePlayState.Camera.GetCellY();
                Tile tile = GamePlayState.TileMap.GetTile(x, y);
                if (tile != null && tile.IsWalkable)
                {
                    SetDestTile(tile, 
                        GamePlayState.TileMap.GetTileGraph().Nodes, 
                        GamePlayState.TileMap);
                }
            }

            if (currTile.Equals(destTile))
            {
                pathAStar = null;
                return;
            }

            if (nextTile.Equals(currTile))
                nextTile = pathAStar.NextTile;

            float distToTravel = MathUtils.Distance(currTile.X, currTile.Y, nextTile.X, nextTile.Y);

            float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float percThisFrame = distThisFrame / distToTravel;

            movementPerc += percThisFrame;

            if(movementPerc >= 1)
            {
                currTile = nextTile;
                movementPerc = 0;
            }

            Entity.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
            Entity.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;
        }

        public void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (tile.MovementCost == 1.0f)
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
    }
}
