using Game.Board;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

namespace Game.Player
{
    public class Manager : NetworkBehaviour
    {
        public List<Color> colors;
        bool move = false;
        Vector3 moveTarget;

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
            return colors[(int)OwnerClientId];
        }

        public Color Color(ulong owner)
        {
            return colors[(int)owner];
        }

        [ServerRpc]
        public void MoveServerRpc(Vector3 target)
        {
            moveTarget = target;
            move = true;
        }
    }
}
