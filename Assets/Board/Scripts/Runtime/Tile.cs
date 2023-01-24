using UnityEngine;

namespace Game.Board
{
    public enum TileType
    {
        Start,
        Tree,
        Rock,
        Path
    }
    public class Tile : MonoBehaviour
    {
        public TileType type = TileType.Path;

    }
}
