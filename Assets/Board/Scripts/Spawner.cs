using UnityEngine;

namespace Game.Board
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefab;
        public void Spawn()
        {
            Tile[] tiles = GetComponentsInChildren<Game.Board.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].type == TileType.Start && tiles[i].unitManager == null)
                {
                    tiles[i].unitManager = GameObject.Instantiate(prefab, tiles[i].transform).GetComponent<Game.Units.Manager>();
                    tiles[i].unitManager.owner = tiles[i].owner;
                }
            }
        }
    }
}
