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
        List<Tile> neighbors = new List<Tile>();
        bool selected = false;

        [ClientRpc]
        public void SetOwerClientRpc()
        {
            Tile tile = transform.parent.GetComponent<Game.Board.Tile>();
            tile.unitManager = this;
            GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId);
            transform.parent.GetComponent<Game.Board.Tile>().RemoveFogOfWar();
        }

        public void OnSelect(GameObject tileGo)
        {
            if (transform.parent.gameObject == tileGo)
            {
                Game.Board.Tile tile = tileGo.GetComponent<Tile>();
                if (tile.GetComponent<NetworkBehaviour>().OwnerClientId == NetworkManager.Singleton.LocalClient.ClientId)
                {
                    NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().MoveServerRpc(tileGo.transform.position);
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.red);

                    neighbors = tile.FindNeighbors(moveRange.Value);

                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.white);
                    }

                    CheckShootTargets();

                    selected = true;
                }
            }

            if (selected)
            {
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (tileGo == neighbors[i].gameObject)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
                    }
                }
            }
        }

        public void OnDeSelect(GameObject tileGo)
        {
            if (selected)
            {
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (tileGo == neighbors[i].gameObject)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.white);
                    }
                }
            }
        }

        public void OnSelectStopped(GameObject tileGo)
        {
            if (selected)
            {
                if (transform.parent.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClient.ClientId)
                {
                    transform.parent.GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color();
                } else
                {
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
                }

                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].GetComponent<NetworkObject>().OwnerClientId != 6)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(neighbors[i].GetComponent<NetworkObject>().OwnerClientId);
                    } else
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
                    }
                    if (neighbors[i].gameObject == tileGo)
                    {
                        MoveServerRpc(tileGo.GetComponent<NetworkBehaviour>());
                    }
                }
                neighbors.Clear();
                selected = false;
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
            transform.parent.GetComponent<Tile>().unitManager = null;
            if (tileGoRef.TryGet<NetworkBehaviour>(out NetworkBehaviour tileGo))
            {
                transform.parent = tileGo.transform;
                transform.position = tileGo.transform.position;
                tileGo.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
                tileGo.GetComponent<Tile>().UpdateClientsClientRpc();

                tileGo.GetComponent<Tile>().unitManager = this;
                tileGo.GetComponent<Tile>().RemoveFogOfWar();
            }
        }
    }
}
