using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public class Spawner : NetworkBehaviour
    {
        public GameObject prefab;
        public NetworkVariable<int> turn = new NetworkVariable<int>();
        public float turnTime = 5;
        float elapsedTime = 0;

        private void Update()
        {
            if (IsServer)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= turnTime)
                {
                    elapsedTime = 0;
                    turn.Value += 1;
                    Spawn();
                }
            }
        }

        public void Spawn()
        {
            Tile[] tiles = GetComponentsInChildren<Game.Board.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].type.Value == TileType.Start && tiles[i].unitManager == null)
                {
                    GameObject go = GameObject.Instantiate(prefab, tiles[i].transform);
                    go.GetComponent<NetworkObject>().Spawn();
                    go.GetComponent<NetworkObject>().TrySetParent(tiles[i].transform);
                    go.GetComponent<NetworkObject>().ChangeOwnership(tiles[i].GetComponent<NetworkObject>().OwnerClientId);
                    go.GetComponent<Units.Manager>().SetOwerClientRpc();
                }
            }
        }
    }
}
