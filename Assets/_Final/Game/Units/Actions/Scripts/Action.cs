using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units.Actions
{
    public abstract class Action : ScriptableObject
    {
        public int range;
        public List<Tile> targets;

        private void OnEnable()
        {
            targets = new List<Tile>();
        }

        public abstract void Evaluate(Units.Manager unit);
        public abstract void Execute(Selector.Manager selector, Units.Manager unit);
        public abstract void Clear();
        public abstract void OnSelectionMove(Selector.Manager selector, Units.Manager unit);
    }
}
