using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units
{
    public class Manager : NetworkBehaviour
    {
        public List<Actions.Action> Actions;

        [ClientRpc]
        public void SetOwerClientRpc()
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(OwnerClientId));
            GetComponent<MeshRenderer>().material.SetInt("_Show", 1);
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
                Actions[i].OnSelectionMove(selector);
            }
        }
    }
}
