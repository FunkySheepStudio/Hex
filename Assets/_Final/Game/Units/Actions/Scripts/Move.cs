using Game.Board;
using log4net.Util;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

namespace Game.Units.Actions
{
    [CreateAssetMenu(menuName = "Game/Units/Actions/Move")]
    public class Move : Action
    {
        public override void Evaluate(Manager unit)
        {
            List<Tile> neighbors = new List<Tile>();
            Tile[] tiles = unit.transform.parent.parent.GetComponentsInChildren<Tile>();
            Tile currentTile = unit.transform.parent.GetComponent<Tile>();

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
        }

        public override void OnSelectionMove(Selector.Manager selector)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (selector.moveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.yellow);
                }

                if (selector.lastMoveSelectedTile != null && selector.lastMoveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
                }
            }
        }

        public override void Execute(Selector.Manager selector, Units.Manager unit)
        {
            unit.transform.parent.GetComponent<Tile>().MoveUnitServerRpc(selector.lastMoveSelectedTile.GetComponent<NetworkBehaviour>());
            /*unit.GetComponent<NetworkObject>().TrySetParent(selector.lastMoveSelectedTile.transform);
            unit.transform.position = selector.lastMoveSelectedTile.transform.position;*/

            /*if (tileGoRef.TryGet<NetworkBehaviour>(out NetworkBehaviour tileGo))
            {
                unit.GetComponent<NetworkObject>().TrySetParent(tileGo.transform);
                //transform.SetParent(tileGo.transform);
                unit.transform.position = tileGo.transform.position;
                //tileGo.GetComponent<NetworkObject>().ChangeOwnership(unit.OwnerClientId);
                //tileGo.GetComponent<Tile>().ShowOwnerClientRpc();
            }*/
        }
    }
}
