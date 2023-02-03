using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

namespace Game.Units
{
    public class Manager : NetworkBehaviour
    {
        public List<Actions.Action> Actions;
        public NetworkVariable<int> moveRange = new NetworkVariable<int>(1);
        public NetworkVariable<int> shootRange = new NetworkVariable<int>(3);
        List<Tile> moveTargets = new List<Tile>();

        [ClientRpc]
        public void SetOwerClientRpc()
        {
            GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId);
        }
        private void Update()
        {
            Vector3 startPosition = transform.position + Vector3.up * 0.30f;
            Debug.DrawLine(startPosition, startPosition + Vector3.left * 3, Color.green);
            Debug.DrawLine(startPosition, startPosition + Vector3.right * 3, Color.red);
            Debug.DrawLine(startPosition, startPosition + (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2) * 3, Color.blue);
            Debug.DrawLine(startPosition, startPosition - (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2) * 3, Color.yellow);
            Debug.DrawLine(startPosition, startPosition + (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2) * 3, Color.cyan);
            Debug.DrawLine(startPosition, startPosition - (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2) * 3, Color.magenta);

            /*


            if (Physics.Raycast(startPosition, startPosition + (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult4, range, mask))
                AddTile(raycastResult4.collider.gameObject);
            if (Physics.Raycast(startPosition, startPosition - (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult5, range, mask))
                AddTile(raycastResult5.collider.gameObject);*/
        }

        public void OnSelect(Tile tile)
        {
            moveTargets = tile.FindMoveLocations(moveRange.Value);

            for (int i = 0; i < moveTargets.Count; i++)
            {
                moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 1);
                moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
            }

            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Evaluate(this);
            }
        }

        public void OnDeSelect(Selector.Manager selector)
        {
            for (int i = 0; i < moveTargets.Count; i++)
            {
                moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 0);

                if (selector.lastMoveSelectedTile != null && selector.lastMoveSelectedTile == moveTargets[i].gameObject)
                {
                    MoveServerRpc(selector.lastMoveSelectedTile.GetComponent<NetworkBehaviour>());
                }
            }

            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Clear();
            }

            moveTargets.Clear();
        }

        public void OnSelectionMove(Selector.Manager selector)
        {
            for (int i = 0; i < moveTargets.Count; i++)
            {
                if (selector.moveSelectedTile == moveTargets[i].gameObject)
                {
                    moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.yellow);
                }

                if (selector.lastMoveSelectedTile != null && selector.lastMoveSelectedTile == moveTargets[i].gameObject)
                {
                    moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
                }
            }
        }

        [ServerRpc]
        void MoveServerRpc(NetworkBehaviourReference tileGoRef)
        {
            if (tileGoRef.TryGet<NetworkBehaviour>(out NetworkBehaviour tileGo))
            {
                GetComponent<NetworkObject>().TrySetParent(tileGo.transform);
                //transform.SetParent(tileGo.transform);
                transform.position = tileGo.transform.position;
                tileGo.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
                tileGo.GetComponent<Tile>().ShowOwnerClientRpc();
            }
        }
    }
}
