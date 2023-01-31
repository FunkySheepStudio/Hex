using System.Collections.Generic;
using Unity.Netcode;
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
    public class Tile : NetworkBehaviour
    {
        public NetworkVariable<Vector3Int> position = new NetworkVariable<Vector3Int>();
        public NetworkVariable<TileType> type = new NetworkVariable<TileType>();
        public bool selected = false;
        public Game.Units.Manager unitManager;
        public GameObject fogOfWar;

        public List<Tile> FindNeighbors(int range = 1)
        {
            List<Tile> neighbors = new List<Tile>();
            Tile[] tiles = transform.parent.GetComponentsInChildren<Tile>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].gameObject != gameObject &&
                    tiles[i].unitManager == null &&
                    tiles[i].type.Value != TileType.Rock &&
                    tiles[i].type.Value != TileType.Start &&
                    Mathf.Abs(position.Value.x - tiles[i].position.Value.x) <= range &&
                    Mathf.Abs(position.Value.y - tiles[i].position.Value.y) <= range &&
                    Mathf.Abs(position.Value.z - tiles[i].position.Value.z) <= range
                )
                {
                    neighbors.Add(tiles[i]);
                }
            }

            return neighbors;
        }

        [ClientRpc]
        public void UpdateClientsClientRpc()
        {
            GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId);
            GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
        }

        public void RemoveFogOfWar()
        {
            if (OwnerClientId == NetworkManager.Singleton.LocalClient.ClientId)
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
