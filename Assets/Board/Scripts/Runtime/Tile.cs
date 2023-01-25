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

    [AddComponentMenu("Game/Tiles/Tile")]
    public class Tile : MonoBehaviour
    {
        public TileType type = TileType.Path;
        public bool selected = false;
        public int owner = -1;

        private void Awake()
        {
            owner = -1;
        }

        public void Select()
        {
            if (!selected)
            {
                GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                selected = true;
            }
        }

        public void DeSelect()
        {
            if (selected)
            {
                GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
                selected = false;
            }
        }
    }
}
