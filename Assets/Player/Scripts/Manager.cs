using Game.Board;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.Player
{
    public class Manager : FunkySheep.Types.Singleton<Manager>
    {
        public Game.Board.Generator boardGenerator;
        public Game.Board.FogOfWar fogOfWar;
        public int id = 0;
        public List<Color> colors;
        public CinemachineVirtualCamera cam;
        bool move = false;
        Vector3 moveTarget;

        private void Start()
        {
            boardGenerator.Generate();
            fogOfWar.Generate(id);
            SetStartPosition();
        }

        private void Update()
        {
            if (move)
            {
                transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime);
                if (transform.position == moveTarget)
                    move = false;
            }
        }

        public void SetStartPosition()
        {
            Tile[] tiles = boardGenerator.GetComponentsInChildren<Game.Board.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].owner == id)
                {
                    transform.position = tiles[i].transform.position;
                }
            }
        }

        public Color Color()
        {
            return colors[id];
        }

        public Color Color(int owner)
        {
            return colors[owner];
        }

        public void Move(Vector3 target)
        {
            moveTarget = target;
            move = true;
        }
    }
}
