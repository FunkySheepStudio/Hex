using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Board
{
    public class TileSelector : MonoBehaviour
    {
        public Camera cam;
        public FunkySheep.Events.GameObjectEvent selectedEvent;
        public FunkySheep.Events.GameObjectEvent deSelectedEvent;
        public FunkySheep.Events.GameObjectEvent stopSelectedEvent;

        GameObject lastTile;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            if (Touch.activeFingers.Count == 1)
            {
                Touch activeTouch = Touch.activeFingers[0].currentTouch;

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(activeTouch.screenPosition);

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject tile = hit.transform.gameObject;

                    if (tile != lastTile)
                    {
                        if (lastTile != null)
                            deSelectedEvent.Raise(lastTile);
                        selectedEvent.Raise(tile);
                        lastTile = tile;
                    }

                    if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        lastTile = null;
                        stopSelectedEvent.Raise(tile);
                    }
                }
            }
        }
    }
}
