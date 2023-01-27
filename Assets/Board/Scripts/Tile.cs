using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
    public enum TileType
    {
        Start,
        Tree,
        Rock,
        Path
    }

    [AddComponentMenu("Game/Tiles/Tile")]
    public class Tile : MonoBehaviour
    {
        public Vector3Int position;
        public TileType type = TileType.Path;
        public bool selected = false;
        public int owner = -1;
        public Game.Units.Manager unitManager;
        public GameObject fogOfWar;

        private void Awake()
        {
            owner = -1;
        }

        public List<Tile> FindNeighbors(int range = 1)
        {
            List<Tile> neighbors = new List<Tile>();
            Tile[] tiles = transform.parent.GetComponentsInChildren<Tile>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].gameObject != gameObject &&
                    tiles[i].unitManager == null &&
                    Mathf.Abs(position.x - tiles[i].position.x) <= range &&
                    Mathf.Abs(position.y - tiles[i].position.y) <= range &&
                    Mathf.Abs(position.z - tiles[i].position.z) <= range
                )
                {
                    neighbors.Add(tiles[i]);
                }
            }

            return neighbors;
        }

        public void RemoveFogOfWar()
        {
            if (owner == Player.Manager.Instance.id)
            {
                List<Tile> tiles = FindNeighbors(2);
                for (int i = 0; i < tiles.Count; i++)
                {
                    GameObject.Destroy(tiles[i].fogOfWar);
                }
            }
        }
    }
}
