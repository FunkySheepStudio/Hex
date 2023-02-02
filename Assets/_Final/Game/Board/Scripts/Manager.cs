using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public class Manager : NetworkBehaviour
    {
        public GameObject prefab;
        private void Start()
        {
            if (IsServer)
            {
                GameObject board = GameObject.Instantiate(prefab, transform);
                board.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
