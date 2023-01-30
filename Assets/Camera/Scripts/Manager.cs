using Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace Game.Cameras
{
    public class Manager : MonoBehaviour
    {
        private void Awake()
        {
            CinemachineVirtualCamera cam = GetComponent<CinemachineVirtualCamera>();
            cam.LookAt = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
            cam.Follow = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
        }
    }
}
