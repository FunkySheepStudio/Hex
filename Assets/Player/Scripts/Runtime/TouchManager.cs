using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class TouchManager : MonoBehaviour
    {
        PlayerInputs inputActions;
        Touch primaryTouchStart;
        Touch primaryTouchStop;

        private void Awake()
        {
            inputActions = new PlayerInputs();
        }

        private void Start()
        {
            inputActions.Touchs.Primary_Touch.started += PrimaryTouchStart;
            inputActions.Touchs.Primary_Touch.canceled += PrimaryTouchStop;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        void PrimaryTouchStart(InputAction.CallbackContext action)
        {
            primaryTouchStart = action.ReadValue<Touch>();
            Debug.Log("Start");
        }

        void PrimaryTouchStop(InputAction.CallbackContext action)
        {
            primaryTouchStop = action.ReadValue<Touch>();
            DefinePrimaryTouch();
            Debug.Log("End");
        }

        void DefinePrimaryTouch()
        {
            Debug.Log(primaryTouchStop.position - primaryTouchStart.position);
        }
    }
}
