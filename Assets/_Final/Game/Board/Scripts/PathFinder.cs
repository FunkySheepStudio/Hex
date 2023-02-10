using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
    public class PathFinder : MonoBehaviour
    {
        public List<Tile> FindPath(Tile startTile, Tile endTile)
        {
            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(startTile);
            Dictionary<Tile, Tile> came_from = new Dictionary<Tile, Tile>();
            came_from[startTile] = null;
            Tile current;

            // Breadth First Search https://www.redblobgames.com/pathfinding/a-star/introduction.html
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();

                if (current == endTile)
                    break;

                foreach (Tile next in findNeighbors(current))
                {
                    if (!came_from.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        came_from[next] = current;
                    }
                }
            }

            //Fill the path and return it
            current = endTile;
            List<Tile> path = new List<Tile>();

            while (current != startTile)
            {
                path.Add(current);
                current = came_from[current];
            }
            path.Add(startTile);
            path.Reverse();

            return path;
        }

        List<Tile> findNeighbors(Tile tile)
        {
            List<Tile> neighbors = new List<Tile>();
            List<Tile> tiles = new List<Tile>();
            GetComponentsInChildren<Tile>(tiles);

            foreach (Tile currentTile in tiles)
            {
                if (
                    Mathf.Abs(tile.position.Value.x - currentTile.position.Value.x) <= 1 &&
                    Mathf.Abs(tile.position.Value.y - currentTile.position.Value.y) <= 1 &&
                    Mathf.Abs(tile.position.Value.z - currentTile.position.Value.z) <= 1
                    )
                {
                    neighbors.Add(currentTile);
                }
            }

            return neighbors;
        }
    }

}
