using UnityEngine;

namespace Game
{
    public class Manager : MonoBehaviour
    {
        public int turn = 0;
        public float turnTime = 1;
        public Game.Board.Spawner spawner;
        float elapsedTime = 0;


        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= turnTime)
            {
                elapsedTime = 0;
                turn += 1;
                spawner.Spawn();
            }
        }
    }
}
