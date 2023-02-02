using FunkySheep.Types;
using Game.Modes;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class Manager : Singleton<Manager>
    {
        public Mode currentMode;
    }
}
