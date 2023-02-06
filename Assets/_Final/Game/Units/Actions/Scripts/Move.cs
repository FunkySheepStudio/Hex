using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

namespace Game.Units.Actions
{
    public class Move : Action
    {
        public AudioClip audioClip;
        GameObject simulation;
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public override void Evaluate()
        {
            Tile[] tiles = transform.parent.parent.GetComponentsInChildren<Tile>();
            Tile currentTile = transform.parent.GetComponent<Tile>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].gameObject != currentTile.gameObject &&
                    tiles[i].GetComponentInChildren<Units.Manager>() == null &&
                    tiles[i].type.Value != TileType.Rock &&
                    tiles[i].type.Value != TileType.Start &&
                    Mathf.Abs(currentTile.position.Value.x - tiles[i].position.Value.x) <= range &&
                    Mathf.Abs(currentTile.position.Value.y - tiles[i].position.Value.y) <= range &&
                    Mathf.Abs(currentTile.position.Value.z - tiles[i].position.Value.z) <= range
                )
                {
                    AddTile(tiles[i]);
                }
            }
        }

        void AddTile(Tile target)
        {
            target.GetComponent<MeshRenderer>().materials[2].color = Color.blue;
            target.GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 1);
            targets.Add(target);
        }

        public override void Clear()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                Tile targetTile = targets[i].GetComponent<Tile>();
                targetTile.GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 0);
            }
            targets.Clear();
            GameObject.Destroy(simulation);
        }

        public override void OnSelectionMove(Selector.Manager selector)
        {
            GameObject.Destroy(simulation);

            for (int i = 0; i < targets.Count; i++)
            {
                if (selector.moveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.yellow);
                    Simulate(i);
                }

                if (selector.lastMoveSelectedTile != null && selector.lastMoveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
                }
            }
        }

        void Simulate(int targetIndex)
        {
            simulation = GameObject.Instantiate(gameObject);
            simulation.GetComponent<MeshCollider>().enabled = false;
            simulation.transform.position = targets[targetIndex].transform.position;
            float fill = simulation.GetComponent<MeshRenderer>().material.GetFloat("_Fill");
            simulation.GetComponent<MeshRenderer>().material.SetFloat("_Fill", fill - 0.1f);
        }

        public override void Execute(Selector.Manager selector)
        {
            if (selector.lastMoveSelectedTile != null)
            {
                MoveUnitServerRpc(selector.lastMoveSelectedTile.GetComponent<NetworkBehaviour>());
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        [ServerRpc]
        public void MoveUnitServerRpc(NetworkBehaviourReference destinationTile)
        {
            if (destinationTile.TryGet<NetworkBehaviour>(out NetworkBehaviour tileDestinationGo))
            {
                unit.health.Value -= 0.1f;
                unit.GetComponent<NetworkObject>().TrySetParent(tileDestinationGo.transform);
                unit.transform.position = tileDestinationGo.transform.position;
                tileDestinationGo.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
                tileDestinationGo.GetComponent<Tile>().ShowOwnerClientRpc();
            }
        }
    }
}
