using Game.Board;
using UnityEngine;

namespace Game.Units.Actions
{
    public class Attack : Action
    {
        public override void Evaluate()
        {
            Vector3 startPosition = transform.position + Vector3.up * 0.30f;
            int mask = LayerMask.GetMask("Units");
                        
            if (Physics.Raycast(startPosition, Vector3.right, out var raycastResult1, range, mask))
            {
                AddTile(raycastResult1.collider.gameObject);
            }
            if (Physics.Raycast(startPosition, Vector3.left, out var raycastResult0, range, mask))
            {
                AddTile(raycastResult0.collider.gameObject);
            }
            if (Physics.Raycast(startPosition, (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult2, range, mask))
            {
                AddTile(raycastResult2.collider.gameObject);
            }
            if (Physics.Raycast(startPosition, -(Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult3, range, mask))
            {
                AddTile(raycastResult3.collider.gameObject);
            }
            if (Physics.Raycast(startPosition, (Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult4, range, mask))
            {
                AddTile(raycastResult4.collider.gameObject);
            }
            if (Physics.Raycast(startPosition, -(Vector3.right * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2), out var raycastResult5, range, mask))
            {
                AddTile(raycastResult5.collider.gameObject);
            }
        }

        void AddTile(GameObject targetUnit)
        {
            if (targetUnit.gameObject != gameObject)
            {
                Tile targetTile = targetUnit.transform.parent.GetComponent<Tile>();
                targetTile.GetComponent<MeshRenderer>().materials[2].color = Color.magenta;
                targetTile.GetComponent<MeshRenderer>().materials[2].SetInt("_Show", 1);
                targets.Add(targetTile);
            }
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
        }

        public override void Execute(Selector.Manager selector)
        {
        }
    }
}
