//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_MAIN/Input/VisualNovelInput.inputactions
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

public partial class @VisualNovelInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @VisualNovelInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""VisualNovelInput"",
    ""maps"": [
        {
            ""name"": ""General"",
            ""id"": ""1d27f79a-30a6-4e40-a661-cec03dfcbf1c"",
            ""actions"": [
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""d30ade9e-8f87-4e1e-896d-1dcc24a7164c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HistoryForward"",
                    ""type"": ""Button"",
                    ""id"": ""1530cad0-0e7a-4c39-8a6e-bfa4e6d10aa4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HistoryBack"",
                    ""type"": ""Button"",
                    ""id"": ""40ab8dfc-55df-46c5-9a14-e80a58bf80df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HistoryLogs"",
                    ""type"": ""Button"",
                    ""id"": ""b6bd192a-a46f-468d-9262-580be092f88b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d6baf9e3-210e-42e2-b0b2-b2157bc351d0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bf050e9-1d95-4174-8a1d-1a59094e7d88"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa2affd1-744f-4858-a85f-7d17fefee2a3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8a97067-917c-4524-8a88-824a75c5959c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HistoryForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e8e30f2-f5cb-4851-b941-13be760ca98f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HistoryBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1467fd1e-c60f-448c-8c5e-b809413a6ad4"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HistoryLogs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // General
        m_General = asset.FindActionMap("General", throwIfNotFound: true);
        m_General_Next = m_General.FindAction("Next", throwIfNotFound: true);
        m_General_HistoryForward = m_General.FindAction("HistoryForward", throwIfNotFound: true);
        m_General_HistoryBack = m_General.FindAction("HistoryBack", throwIfNotFound: true);
        m_General_HistoryLogs = m_General.FindAction("HistoryLogs", throwIfNotFound: true);
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

    // General
    private readonly InputActionMap m_General;
    private List<IGeneralActions> m_GeneralActionsCallbackInterfaces = new List<IGeneralActions>();
    private readonly InputAction m_General_Next;
    private readonly InputAction m_General_HistoryForward;
    private readonly InputAction m_General_HistoryBack;
    private readonly InputAction m_General_HistoryLogs;
    public struct GeneralActions
    {
        private @VisualNovelInput m_Wrapper;
        public GeneralActions(@VisualNovelInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Next => m_Wrapper.m_General_Next;
        public InputAction @HistoryForward => m_Wrapper.m_General_HistoryForward;
        public InputAction @HistoryBack => m_Wrapper.m_General_HistoryBack;
        public InputAction @HistoryLogs => m_Wrapper.m_General_HistoryLogs;
        public InputActionMap Get() { return m_Wrapper.m_General; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralActions set) { return set.Get(); }
        public void AddCallbacks(IGeneralActions instance)
        {
            if (instance == null || m_Wrapper.m_GeneralActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GeneralActionsCallbackInterfaces.Add(instance);
            @Next.started += instance.OnNext;
            @Next.performed += instance.OnNext;
            @Next.canceled += instance.OnNext;
            @HistoryForward.started += instance.OnHistoryForward;
            @HistoryForward.performed += instance.OnHistoryForward;
            @HistoryForward.canceled += instance.OnHistoryForward;
            @HistoryBack.started += instance.OnHistoryBack;
            @HistoryBack.performed += instance.OnHistoryBack;
            @HistoryBack.canceled += instance.OnHistoryBack;
            @HistoryLogs.started += instance.OnHistoryLogs;
            @HistoryLogs.performed += instance.OnHistoryLogs;
            @HistoryLogs.canceled += instance.OnHistoryLogs;
        }

        private void UnregisterCallbacks(IGeneralActions instance)
        {
            @Next.started -= instance.OnNext;
            @Next.performed -= instance.OnNext;
            @Next.canceled -= instance.OnNext;
            @HistoryForward.started -= instance.OnHistoryForward;
            @HistoryForward.performed -= instance.OnHistoryForward;
            @HistoryForward.canceled -= instance.OnHistoryForward;
            @HistoryBack.started -= instance.OnHistoryBack;
            @HistoryBack.performed -= instance.OnHistoryBack;
            @HistoryBack.canceled -= instance.OnHistoryBack;
            @HistoryLogs.started -= instance.OnHistoryLogs;
            @HistoryLogs.performed -= instance.OnHistoryLogs;
            @HistoryLogs.canceled -= instance.OnHistoryLogs;
        }

        public void RemoveCallbacks(IGeneralActions instance)
        {
            if (m_Wrapper.m_GeneralActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGeneralActions instance)
        {
            foreach (var item in m_Wrapper.m_GeneralActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GeneralActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GeneralActions @General => new GeneralActions(this);
    public interface IGeneralActions
    {
        void OnNext(InputAction.CallbackContext context);
        void OnHistoryForward(InputAction.CallbackContext context);
        void OnHistoryBack(InputAction.CallbackContext context);
        void OnHistoryLogs(InputAction.CallbackContext context);
    }
}