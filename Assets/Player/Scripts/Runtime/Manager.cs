using Game.Board;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public enum Action
    {
        None,
        MovingUnit
    }

    public class Manager : FunkySheep.Types.Singleton<Manager>
    {
        public Game.Board.Generator boardGenerator;
        public Game.Board.FogOfWar fogOfWar;
        public int id = 0;
        public List<Color> colors;
        public Action action = Action.None;

        private void Start()
        {
            boardGenerator.Generate();
            fogOfWar.Generate(id);
            SetStartPosition();
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
    }
}
