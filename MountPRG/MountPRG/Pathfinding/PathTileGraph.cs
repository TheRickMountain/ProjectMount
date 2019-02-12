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

        public PathTileGraph(TileLayer tileLayer)
        {
            Nodes = new Dictionary<Tile, Node<Tile>>();

            for(int x = 0; x < tileLayer.Width; x++)
            {
                for(int y = 0; y < tileLayer.Height; y++)
                {
                    Tile t = tileLayer.GetTile(x, y);

                    Node<Tile> n = new Node<Tile>();
                    n.data = t;
                    Nodes.Add(t, n);
                }
            }
        }

    }
}
