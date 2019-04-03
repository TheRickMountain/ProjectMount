using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class PathAStar
    {
        private Stack<Tile> path;

        public PathAStar(Tile tileStart, Tile tileEnd, Dictionary<Tile, Node<Tile>> nodes, TileMap tileMap)
        {
            if (!nodes.ContainsKey(tileStart) || !nodes.ContainsKey(tileEnd))
            {
                Console.WriteLine("Node hasn't start or end tile");
                return;
            }

            Node<Tile> startNode = nodes[tileStart];

            Node<Tile> targetNode = nodes[tileEnd];

            Heap<Node<Tile>> openSet = new Heap<Node<Tile>>(nodes.Count);
            HashSet<Node<Tile>> closedSet = new HashSet<Node<Tile>>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node<Tile> currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode.Equals(targetNode))
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Tile n in currentNode.data.GetNeighbours(true))
                {
                    Node<Tile> neighbourNode = nodes[n];

                    if (n == null)
                        continue;

                    if (IsClippingCorner(currentNode.data, n, tileMap))
                        continue;

                    if (!n.Walkable || closedSet.Contains(neighbourNode))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode);
                    if (newMovementCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.gCost = newMovementCostToNeighbour;
                        neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                        neighbourNode.parent = currentNode;

                        if (!openSet.Contains(neighbourNode))
                            openSet.Add(neighbourNode);
                        else
                            openSet.UpdateItem(neighbourNode);
                    }
                }
            }
        }

        private void RetracePath(Node<Tile> startNode, Node<Tile> endNode)
        {
            path = new Stack<Tile>();
            Node<Tile> currentNode = endNode;

            while (!currentNode.Equals(startNode))
            {
                path.Push(currentNode.data);
                currentNode = currentNode.parent;
            }
        }

        public int Length
        {
            get
            {
                if (path != null)
                    return path.Count;

                return -1;
            }
        }

        public Tile NextTile
        {
            get
            {
                return path.Pop();
            }
        }

        private int GetDistance(Node<Tile> nodeA, Node<Tile> nodeB)
        {
            int dstX = (int)Math.Abs(nodeA.data.X - nodeB.data.X);
            int dstY = (int)Math.Abs(nodeA.data.Y - nodeB.data.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);

            return 14 * dstX + 10 * (dstY - dstX);
        }

        private bool IsClippingCorner(Tile curr, Tile neigh, TileMap tilemap)
        {
            int dX = curr.X - neigh.X;
            int dY = curr.Y - neigh.Y;

            if (Math.Abs(dX) + Math.Abs(dY) == 2)
            {

                if (!tilemap.GetTile(curr.X - dX, curr.Y).Walkable)
                    return true;

                if (!tilemap.GetTile(curr.X, curr.Y - dY).Walkable)
                    return true;

            }

            return false;
        }

        public List<Tile> GetList()
        {
            if (path == null)
                return null;

            return path.ToList();
        }

        public Stack<Tile> GetStack()
        {
            return path;
        }

    }
}
