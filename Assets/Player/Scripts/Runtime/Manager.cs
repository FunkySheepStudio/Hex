using Game.Board;
using UnityEngine;

namespace Game.Player
{
    public class Manager : MonoBehaviour
    {
        public Game.Board.Generator boardGenerator;
        public Game.Board.FogOfWar fogOfWar;
        public int id = 0;

        private void Start()
        {
            boardGenerator.Generate();
            fogOfWar.Generate(id);
            SetStartPosition();
        }

        public void SetStartPosition()
        {
            Tile[] tiles = boardGenerator.GetComponentsInChildren<Game.Board.Tile>();
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
