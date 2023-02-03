using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units
{
    public class Manager : NetworkBehaviour
    {
        public NetworkVariable<int> moveRange = new NetworkVariable<int>(1);
        public NetworkVariable<int> shootRange = new NetworkVariable<int>(3);
        List<Tile> moveTargets = new List<Tile>();

        [ClientRpc]
        public void SetOwerClientRpc()
        {
            GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId);
        }

        public void OnSelect(Tile tile)
        {
            moveTargets = tile.FindMoveLocations(moveRange.Value);

            for (int i = 0; i < moveTargets.Count; i++)
            {
                moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 1);
                moveTargets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
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

        void CheckShootTargets()
        {
            int mask = LayerMask.GetMask("Units");
            if (Physics.Raycast(transform.position, transform.position + Vector3.left, out var raycastResult0, shootRange.Value, mask))
                SetTarget(raycastResult0.collider.gameObject);
            if (Physics.Raycast(transform.position, transform.position + Vector3.right, out var raycastResult1, shootRange.Value, mask))
                SetTarget(raycastResult1.collider.gameObject);
            if (Physics.Raycast(transform.position, transform.position + (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult2, shootRange.Value, mask))
                SetTarget(raycastResult2.collider.gameObject);
            if (Physics.Raycast(transform.position, transform.position - (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult3, shootRange.Value, mask))
                SetTarget(raycastResult3.collider.gameObject);
            if (Physics.Raycast(transform.position, transform.position + (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult4, shootRange.Value, mask))
                SetTarget(raycastResult4.collider.gameObject);
            if (Physics.Raycast(transform.position, transform.position - (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult5, shootRange.Value, mask))
                SetTarget(raycastResult5.collider.gameObject);
        }

        void SetTarget(GameObject target)
        {
            target.transform.parent.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.blue);
            target.transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
        }

        [ServerRpc]
        void MoveServerRpc(NetworkBehaviourReference tileGoRef)
        {
            if (tileGoRef.TryGet<NetworkBehaviour>(out NetworkBehaviour tileGo))
            {
                transform.parent = tileGo.transform;
                transform.position = tileGo.transform.position;
                tileGo.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
                tileGo.GetComponent<Tile>().ShowOwnerClientRpc();
            }
        }
    }
}
