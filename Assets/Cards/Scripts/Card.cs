using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(menuName = "Game/Cards/Card")]
    public class Card : ScriptableObject
    {
        public Texture2D background;
        [Range(1, 7)]
        public int level;
    }
}
