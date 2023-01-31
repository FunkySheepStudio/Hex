using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class Manager : NetworkBehaviour
    {
        public NetworkVariable<int> playerCount = new NetworkVariable<int>();
        public NetworkVariable<bool> started = new NetworkVariable<bool>();
        public GameObject boardPrefab;

        private void Update()
        {
            if ((IsServer || IsHost) && !started.Value && (int)Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count == playerCount.Value)
            {
                Unity.Netcode.NetworkManager.Singleton.SceneManager.LoadScene("Game/Game", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                GameObject board = GameObject.Instantiate(boardPrefab);
                board.GetComponent<NetworkObject>().Spawn();
                started.Value = true;
            }
        }
    }
}
