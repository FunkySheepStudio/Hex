using UnityEngine;

namespace Game.Player
{
    public class Manager : MonoBehaviour
    {
        public Game.Board.Generator generator;
        public int startPosition = 0;

        private void Start()
        {
            generator.Generate();
        }
    }
}
