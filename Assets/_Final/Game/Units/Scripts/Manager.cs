using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units
{
    public class Manager : NetworkBehaviour
    {
        public NetworkVariable<float> health = new NetworkVariable<float>(1);
        public List<Actions.Action> Actions;

        public override void OnNetworkSpawn()
        {
            health.OnValueChanged += (float previous, float current) => {
                GetComponent<MeshRenderer>().material.SetFloat("_Fill", current);
            };
        }

        [ClientRpc]
        public void SetOwerClientRpc()
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId));
        }

        public void OnSelect(Tile tile)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Evaluate(this);
            }
        }

        public void OnDeSelect(Selector.Manager selector)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Execute(selector, this);
                Actions[i].Clear();
            }
        }

        public void OnSelectionMove(Selector.Manager selector)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].OnSelectionMove(selector, this);
            }
        }
    }
}
