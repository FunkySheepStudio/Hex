using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

namespace Game.Units.Actions
{
    [CreateAssetMenu(menuName = "Game/Units/Actions/Move")]
    public class Move : Action
    {
        GameObject simulation;

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
            GameObject.Destroy(simulation);
        }

        public override void OnSelectionMove(Selector.Manager selector, Units.Manager unit)
        {
            GameObject.Destroy(simulation);

            for (int i = 0; i < targets.Count; i++)
            {
                if (selector.moveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.yellow);
                    Simulate(i, unit);
                }

                if (selector.lastMoveSelectedTile != null && selector.lastMoveSelectedTile == targets[i].gameObject)
                {
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.blue);
                }
            }
        }

        void Simulate(int targetIndex, Units.Manager unit)
        {
            simulation = GameObject.Instantiate(unit.gameObject);
            simulation.GetComponent<MeshCollider>().enabled = false;
            simulation.transform.position = targets[targetIndex].transform.position;
            float fill = simulation.GetComponent<MeshRenderer>().material.GetFloat("_Fill");
            simulation.GetComponent<MeshRenderer>().material.SetFloat("_Fill", fill - 0.1f);
        }

        public override void Execute(Selector.Manager selector, Units.Manager unit)
        {
            if (selector.lastMoveSelectedTile != null)
            {
                unit.transform.parent.GetComponent<Tile>().MoveUnitServerRpc(selector.lastMoveSelectedTile.GetComponent<NetworkBehaviour>());
            }
        }
    }
}
