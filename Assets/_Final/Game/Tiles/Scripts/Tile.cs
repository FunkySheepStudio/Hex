using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public enum TileType
    {
        Start,
        Tree,
        Rock,
        Path
    }

    [Serializable]
    [AddComponentMenu("Game/Tiles/Tile")]
    public class Tile : NetworkBehaviour
    {
        public NetworkVariable<Vector3Int> position = new NetworkVariable<Vector3Int>();
        public NetworkVariable<TileType> type = new NetworkVariable<TileType>();

        private void Start()
        {
            ShowOwnerClientRpc();
        }

        [ClientRpc]
        public void ShowOwnerClientRpc()
        {
            ulong ownerId = GetComponent<NetworkObject>().OwnerClientId;
            if (ownerId != 6)
            {
                GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(ownerId);
                GetComponent<MeshRenderer>().materials[1].SetInt("_Show", 1);
            } else
            {
                GetComponent<MeshRenderer>().materials[1].SetInt("_Show", 0);
            }
        }

        [ServerRpc]
        public void MoveUnitServerRpc(NetworkBehaviourReference destinationTile)
        {
            if (destinationTile.TryGet<NetworkBehaviour>(out NetworkBehaviour tileDestinationGo))
            {
                Units.Manager unit = GetComponentInChildren<Units.Manager>();
                unit.GetComponent<NetworkObject>().TrySetParent(tileDestinationGo.transform);
                unit.transform.position = tileDestinationGo.transform.position;
                tileDestinationGo.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
                tileDestinationGo.GetComponent<Tile>().ShowOwnerClientRpc();
            }
        }

        public void OnTileSelection(GameObject selectorManager)
        {
            if (selectorManager.GetComponent<Selector.Manager>().selectedTile == gameObject && IsOwner
            )
            {
                GetComponent<MeshRenderer>().materials[2].color = Color.green;
                GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 1);

                Units.Manager unit = GetComponentInChildren<Units.Manager>();
                if (unit != null)
                {
                    unit.OnSelect(this);
                }
            }
        }

        public void OnTileDeSelection(GameObject selectorManager)
        {
            Selector.Manager selector = selectorManager.GetComponent<Selector.Manager>();
            if (selector.selectedTile == gameObject && IsOwner)
            {
                GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 0);

                Units.Manager unit = GetComponentInChildren<Units.Manager>();
                if (unit != null)
                {
                    unit.OnDeSelect(selector);
                }
            }
        }

        public void OnTileSelectionMoved(GameObject selectorManager)
        {
            Selector.Manager selector = selectorManager.GetComponent<Selector.Manager>();
            if (selector.selectedTile == gameObject && IsOwner)
            {
                Units.Manager unit = GetComponentInChildren<Units.Manager>();
                if (unit != null)
                {
                    unit.OnSelectionMove(selector);
                }
            }
        }
    }
}
