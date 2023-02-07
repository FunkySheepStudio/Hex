using Game.Board;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units.Actions
{
    public class Attack : Action
    {
        public GameObject prefab;
        GameObject simulation;
        bool IsTransfering = false;
        GameObject remoteTile = null;

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
                    targets[i].GetComponent<MeshRenderer>().materials[2].SetColor("_Color", Color.magenta);
                }
            }
        }

        void Simulate(int targetIndex)
        {
            simulation = GameObject.Instantiate(prefab);
            simulation.transform.position = transform.position + Vector3.up * 0.3f;
            simulation.transform.LookAt(targets[targetIndex].transform.position + Vector3.up * 0.3f);
            simulation.transform.localScale = new Vector3(
                simulation.transform.localScale.x,
                simulation.transform.localScale.y,
                Vector3.Distance(transform.position, targets[targetIndex].transform.position)
            );
            simulation.GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId);
        }

        public override void Execute(Selector.Manager selector)
        {
            GameObject.Destroy(simulation);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].gameObject == selector.lastMoveSelectedTile)
                {
                    AttackUnitServerRpc(selector.lastMoveSelectedTile.GetComponent<NetworkBehaviour>());
                }
            }
        }

        [ServerRpc]
        public void AttackUnitServerRpc(NetworkBehaviourReference destinationTile, ServerRpcParams serverRpcParams = default)
        {
            if (destinationTile.TryGet<NetworkBehaviour>(out NetworkBehaviour tileDestinationGo))
            {
                simulation = GameObject.Instantiate(prefab);
                simulation.transform.position = transform.position + Vector3.up * 0.3f;
                simulation.transform.LookAt(tileDestinationGo.transform.position + Vector3.up * 0.3f);
                simulation.transform.localScale = new Vector3(
                    simulation.transform.localScale.x,
                    simulation.transform.localScale.y,
                    Vector3.Distance(transform.position, tileDestinationGo.transform.position)
                );

                simulation.GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(serverRpcParams.Receive.SenderClientId);

                simulation.GetComponent<NetworkObject>().Spawn();
                simulation.GetComponent<NetworkObject>().TrySetParent(gameObject.transform.parent);
                remoteTile = tileDestinationGo.gameObject;
                IsTransfering = true;
            }
        }

        private void Update()
        {
            if (IsServer && IsTransfering && simulation != null && remoteTile != null)
            {
                Units.Manager localUnit = simulation.transform.parent.GetComponentInChildren<Units.Manager>();
                Units.Manager remoteUnit = remoteTile.GetComponentInChildren<Units.Manager>();
                if (localUnit != null && remoteUnit != null)
                {
                    localUnit.health.Value += Time.deltaTime;
                    remoteUnit.health.Value -= Time.deltaTime;

                    if (localUnit.health.Value >= 1)
                    {
                        DestroyTransfert();
                    }
                } else
                {
                    DestroyTransfert();
                }
            } else
            {
                DestroyTransfert();
            }
        }

        private void DestroyTransfert()
        {
            IsTransfering = false;
            remoteTile = null;
            Destroy(simulation);
        }
    }
}
