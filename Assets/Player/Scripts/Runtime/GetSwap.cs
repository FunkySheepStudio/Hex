using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using FunkySheep.Events;

namespace Game.Player.Inputs
{
    public class GetSwap : MonoBehaviour
    {
        public Vector2IntEvent swipeEvent;
        Vector2? touch0;

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

                if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved && touch0 == null)
                {
                    touch0 = Vector2.zero;
                } else if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    touch0 += activeTouch.delta;
                } else if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                {
                    GetCompassDirection(touch0.Value);
                    touch0 = null;
                }
            }
        }

        private void GetCompassDirection(Vector2 vector)
        {
            Vector2Int[] compass = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
            float maxDot = -Mathf.Infinity;
            Vector2Int direction = Vector2Int.zero;

            for (int i = 0; i < compass.Length; i++)
            {
                var t = Vector2.Dot(vector, compass[i]);
                if (t > maxDot)
                {
                    direction = compass[i];
                    maxDot = t;
                }
            }

            swipeEvent.Raise(direction);
        }

    }
}
