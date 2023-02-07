using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Windows;
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
        SelectorInputs inputs;


        private void Awake()
        {
            inputs = new SelectorInputs();
            cam = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            inputs.Enable();
            inputs.Selector.Point.started += OnMouseDown;
            inputs.Selector.Point.canceled += OnMouseUp;

            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += OnFingerDown;
            Touch.onFingerMove += OnFingerMove;
            Touch.onFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            inputs.Disable();
            EnhancedTouchSupport.Disable();
            Touch.onFingerDown -= OnFingerDown;
            Touch.onFingerMove -= OnFingerMove;
            Touch.onFingerUp -= OnFingerUp;
        }

        private void OnMouseDown(InputAction.CallbackContext ctx)
        {
            Select(Mouse.current.position.ReadValue());
            inputs.Selector.Move.performed += OnMouseMove;
        }

        private void OnMouseUp(InputAction.CallbackContext ctx)
        {
            inputs.Selector.Move.performed -= OnMouseMove;
            DeSelect();
        }

        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            OnMove(ctx.ReadValue<Vector2>());
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

        void Select(Vector2 screenPosition)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(screenPosition);
            LayerMask mask = LayerMask.GetMask("Tiles", "Units");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tiles"))
                {
                    selectedTile = hit.transform.gameObject;
                }
                else
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

        void OnMove(Vector2 screenPosition)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(screenPosition);
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

        void DeSelect()
        {
            deSelectedEvent.Raise(gameObject);
            selectedTile = null;
            moveSelectedTile = null;
            lastMoveSelectedTile = null;
        }
    }
}
