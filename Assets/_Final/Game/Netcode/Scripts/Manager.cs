using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Game.Netcode
{
    public class Manager : MonoBehaviour
    {
        public void StartHost()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.StartHost();
        }

        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        void OnClientConnected(ulong clientId)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count == Game.Manager.Instance.currentMode.playerSettings.Count)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("_Final/Game/Game", LoadSceneMode.Single);
            }
        }
    }
}
