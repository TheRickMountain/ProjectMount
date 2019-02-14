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

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 2f;

        public AIController()
            : base(true, false)
        {
            
        }

        public override void Initialize()
        {
            currTile = destTile = nextTile = GamePlayState.TileMap.GetCollisionLayer()
                .GetTile((int)(Entity.X / Engine.TileWidth), (int)(Entity.Y / Engine.TileHeight));
        }

        public override void Update(GameTime gameTime)
        {
            if(currTile.Equals(destTile))
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

            Entity.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * Engine.TileWidth;
            Entity.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * Engine.TileHeight;
        }

        public void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, CollisionLayer collisionLayer)
        {
            if (tile.MovementCost == 1.0f)
            {
                currTile = nextTile = collisionLayer.GetTile((int)(Entity.X / Engine.TileWidth), 
                    (int)(Entity.Y / Engine.TileHeight));
                pathAStar = new PathAStar(currTile, tile, nodes, collisionLayer);
                if (pathAStar.Length != -1)
                    destTile = tile;
                else
                    pathAStar = null;
            }
        }
    }
}
