using Game.Board;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

namespace Game.Player
{
    public class Manager : NetworkBehaviour
    {
        public int id = 0;
        public List<Color> colors;
        public CinemachineVirtualCamera cam;
        bool move = false;
        Vector3 moveTarget;

        public void Awake()
        {
            id = (int)Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        }

        private void Start()
        {
            NetworkManager.SceneManager.LoadScene("Game/Game", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        private void Update()
        {
            if (move)
            {
                transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime);
                if (transform.position == moveTarget)
                    move = false;
            }
        }

        public Color Color()
        {
            return colors[id];
        }

        public Color Color(int owner)
        {
            return colors[owner];
        }

        public void Move(Vector3 target)
        {
            moveTarget = target;
            move = true;
        }
    }
}
