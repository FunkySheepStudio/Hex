using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Board
{
    public class TileSelector : MonoBehaviour
    {
        public Camera cam;

        Tile lastTile;

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
                    Transform objectHit = hit.transform;
                    Tile tile = objectHit.GetComponent<Tile>();

                    if (tile != lastTile)
                    {
                        if (lastTile != null)
                            lastTile.DeSelect();
                        tile.Select();
                        lastTile = tile;
                    }

                    if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        tile.DeSelect();
                    }
                }
            }
        }
    }
}
