using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modes
{
    [Serializable]
    public class PlayerSettings
    {
        public ulong id;
        public int startPosition;
    }

    [CreateAssetMenu(menuName = "Game/Modes/Mode")]
    public class Mode : ScriptableObject
    {
        public List<PlayerSettings> playerSettings;
    }
}
