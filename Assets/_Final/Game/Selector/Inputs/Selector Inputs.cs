//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/_Final/Game/Selector/Inputs/Selector Inputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @SelectorInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @SelectorInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Selector Inputs"",
    ""maps"": [
        {
            ""name"": ""Selector"",
            ""id"": ""ef495877-934d-4521-831b-674892e5b067"",
            ""actions"": [
                {
                    ""name"": ""Point"",
                    ""type"": ""Value"",
                    ""id"": ""c7fe9a34-982d-462f-ac5c-3ab1041af432"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7cc67053-c15f-4a87-a955-569fe87b20b4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ad93cb57-2fb3-483d-b736-58e949914ee5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b5d2951-c731-4cfc-9a2a-d837b67d67cc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Selector
        m_Selector = asset.FindActionMap("Selector", throwIfNotFound: true);
        m_Selector_Point = m_Selector.FindAction("Point", throwIfNotFound: true);
        m_Selector_Move = m_Selector.FindAction("Move", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Selector
    private readonly InputActionMap m_Selector;
    private ISelectorActions m_SelectorActionsCallbackInterface;
    private readonly InputAction m_Selector_Point;
    private readonly InputAction m_Selector_Move;
    public struct SelectorActions
    {
        private @SelectorInputs m_Wrapper;
        public SelectorActions(@SelectorInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Point => m_Wrapper.m_Selector_Point;
        public InputAction @Move => m_Wrapper.m_Selector_Move;
        public InputActionMap Get() { return m_Wrapper.m_Selector; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectorActions set) { return set.Get(); }
        public void SetCallbacks(ISelectorActions instance)
        {
            if (m_Wrapper.m_SelectorActionsCallbackInterface != null)
            {
                @Point.started -= m_Wrapper.m_SelectorActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_SelectorActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_SelectorActionsCallbackInterface.OnPoint;
                @Move.started -= m_Wrapper.m_SelectorActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_SelectorActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_SelectorActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_SelectorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public SelectorActions @Selector => new SelectorActions(this);
    public interface ISelectorActions
    {
        void OnPoint(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
}
