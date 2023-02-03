using UnityEngine;

namespace Game.Cameras
{
    public class MoveTargetToSelection : MonoBehaviour
    {
        public void Move(GameObject selectorManager)
        {
            transform.position = selectorManager.GetComponent<Selector.Manager>().selectedTile.transform.position;
        }
    }
}
