using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;

namespace Game.Units.Actions
{
    public abstract class Action : NetworkBehaviour
    {
        public int range;
        public List<Tile> targets;
        public Units.Manager unit;

        public override void OnNetworkSpawn()
        {
            unit = GetComponent<Units.Manager>();
            targets = new List<Tile>();
            unit.Actions.Add(this);
        }

        public abstract void Evaluate();
        public abstract void Execute(Selector.Manager selector);
        public abstract void Clear();
        public abstract void OnSelectionMove(Selector.Manager selector);
    }
}
