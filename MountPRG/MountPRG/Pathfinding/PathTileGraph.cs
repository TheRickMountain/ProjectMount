using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class PathTileGraph
    {
        public Dictionary<Tile, Node<Tile>> Nodes;

        public PathTileGraph(CollisionLayer collisionLayer)
        {
            Nodes = new Dictionary<Tile, Node<Tile>>();

            for(int x = 0; x < collisionLayer.Width; x++)
            {
                for(int y = 0; y < collisionLayer.Height; y++)
                {
                    Tile t = collisionLayer.GetTile(x, y);

                    Node<Tile> n = new Node<Tile>();
                    n.data = t;
                    Nodes.Add(t, n);
                }
            }
        }

    }
}
