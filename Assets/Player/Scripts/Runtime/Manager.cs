using Game.Board;
using UnityEngine;

namespace Game.Player
{
    public class Manager : MonoBehaviour
    {
        public Game.Board.Generator generator;
        public int id = 0;

        private void Start()
        {
            generator.Generate();
            SetStartPosition();
        }

        public void SetStartPosition()
        {
            Tile[] tiles = generator.GetComponentsInChildren<Game.Board.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].owner == id)
                {
                    transform.position = tiles[i].transform.position;
                }
            }
        }
    }
}
