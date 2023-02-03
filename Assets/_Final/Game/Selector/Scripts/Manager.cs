using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Selector
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Events.GameObjectEvent selectedEvent;
        public FunkySheep.Events.GameObjectEvent movedEvent;
        public FunkySheep.Events.GameObjectEvent deSelectedEvent;

        public GameObject selectedTile;
        public GameObject moveSelectedTile;
        public GameObject lastMoveSelectedTile;

        Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += OnFingerDown;
            Touch.onFingerMove += OnFingerMove;
            Touch.onFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            Touch.onFingerDown -= OnFingerDown;
            Touch.onFingerMove -= OnFingerMove;
            Touch.onFingerUp -= OnFingerUp;
        }

        void OnFingerDown(Finger finger)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(finger.screenPosition);
            LayerMask mask = LayerMask.GetMask("Tiles", "Units");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tiles"))
                {
                    selectedTile = hit.transform.gameObject;
                } else
                {
                    selectedTile = hit.transform.parent.gameObject;
                }

                selectedEvent.Raise(gameObject);
            }
        }

        void OnFingerMove(Finger finger)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(finger.screenPosition);
            LayerMask mask = LayerMask.GetMask("Tiles", "Units");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tiles"))
                {
                    moveSelectedTile = hit.transform.gameObject;
                }
                else
                {
                    moveSelectedTile = hit.transform.parent.gameObject;
                }

                if (lastMoveSelectedTile != moveSelectedTile)
                {
                    movedEvent.Raise(gameObject);
                    lastMoveSelectedTile = moveSelectedTile;
                }
            }
        }

        void OnFingerUp(Finger finger)
        {
            deSelectedEvent.Raise(gameObject);
            selectedTile = null;
            moveSelectedTile = null;
            lastMoveSelectedTile = null;
        }
    }
}
