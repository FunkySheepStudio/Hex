using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Game.Board;

namespace Game.Player
{
    public class Manager : NetworkBehaviour
    {
        public List<Color> colors;
        bool move = false;
        int moveIndex = 0;
        float elapsedMove = 0;
        List<Tile> path = new List<Tile>();
        public GameObject board;
        public Tile currentTile;

        private void Update()
        {
            if (move)
            {
                elapsedMove += Time.deltaTime;
                if (elapsedMove > 1)
                    elapsedMove = 1;
                transform.position = Vector3.Lerp(currentTile.transform.position, path[moveIndex].transform.position, elapsedMove);
                if (transform.position == path[moveIndex].transform.position)
                {
                    currentTile = path[moveIndex];
                    elapsedMove = 0;
                    moveIndex += 1;
                }
                if (moveIndex == path.Count)
                {
                    path.Clear();
                    elapsedMove = 0;
                    moveIndex = 0;
                    move = false;
                }
            }
        }
   
        public Color Color()
        {
            return colors[(int)OwnerClientId];
        }

        public Color Color(ulong owner)
        {
            return colors[(int)owner];
        }

        public void Move(GameObject selector)
        {
            path = board.GetComponent<Board.PathFinder>().FindPath(currentTile, selector.GetComponent<Selector.Manager>().selectedTile.GetComponent<Tile>());
            move = true;
        }
    }
}
