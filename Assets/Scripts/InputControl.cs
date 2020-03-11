// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/InputControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace StackTicTacToe
{
    public class @InputControl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputControl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""9f75fc75-8335-4361-9472-e1a0ea46e098"",
            ""actions"": [
                {
                    ""name"": ""PointerMove"",
                    ""type"": ""Value"",
                    ""id"": ""3ff76445-0ea7-462f-994b-21424c4d76a8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""26f7eb54-d729-4cc4-913b-229b37341be8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9f14a291-3afb-4b98-8e3c-bd23634a48da"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65abaa32-af5b-4ff7-ba6a-c9382cd192a8"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Main
            m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
            m_Main_PointerMove = m_Main.FindAction("PointerMove", throwIfNotFound: true);
            m_Main_Tap = m_Main.FindAction("Tap", throwIfNotFound: true);
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

        // Main
        private readonly InputActionMap m_Main;
        private IMainActions m_MainActionsCallbackInterface;
        private readonly InputAction m_Main_PointerMove;
        private readonly InputAction m_Main_Tap;
        public struct MainActions
        {
            private @InputControl m_Wrapper;
            public MainActions(@InputControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @PointerMove => m_Wrapper.m_Main_PointerMove;
            public InputAction @Tap => m_Wrapper.m_Main_Tap;
            public InputActionMap Get() { return m_Wrapper.m_Main; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
            public void SetCallbacks(IMainActions instance)
            {
                if (m_Wrapper.m_MainActionsCallbackInterface != null)
                {
                    @PointerMove.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerMove;
                    @PointerMove.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerMove;
                    @PointerMove.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerMove;
                    @Tap.started -= m_Wrapper.m_MainActionsCallbackInterface.OnTap;
                    @Tap.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnTap;
                    @Tap.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnTap;
                }
                m_Wrapper.m_MainActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @PointerMove.started += instance.OnPointerMove;
                    @PointerMove.performed += instance.OnPointerMove;
                    @PointerMove.canceled += instance.OnPointerMove;
                    @Tap.started += instance.OnTap;
                    @Tap.performed += instance.OnTap;
                    @Tap.canceled += instance.OnTap;
                }
            }
        }
        public MainActions @Main => new MainActions(this);
        public interface IMainActions
        {
            void OnPointerMove(InputAction.CallbackContext context);
            void OnTap(InputAction.CallbackContext context);
        }
    }
}
