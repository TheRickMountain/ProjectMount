using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public enum CollisionType
    {
        None = -1,
        Passable = 0,
        Impassable = 1
    }

    public class CollisionLayer
    {
        private TileMap tileMap;

        private int[] collisions;

        public CollisionLayer(TileMap tileMap)
        {
            this.tileMap = tileMap;
            collisions = new int[tileMap.MapWidth * tileMap.MapHeight];
        }

        public int GetCollider(int x, int y)
        {
            if (x < 0 || x >= tileMap.MapWidth || y < 0 || y >= tileMap.MapHeight)
                return -1;

            return collisions[y * tileMap.MapWidth + x];
        }

        public void SetCollider(int x, int y, CollisionType type)
        {
            if (x < 0 || x >= tileMap.MapWidth || y < 0 || y >= tileMap.MapHeight)
                return;

            collisions[y * tileMap.MapWidth + x] = (int)type;
        }
    }
}
