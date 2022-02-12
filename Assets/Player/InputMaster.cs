// GENERATED AUTOMATICALLY FROM 'Assets/Player/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""683e8c73-d6d3-4c31-a3d2-c8c3ddd57fa9"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9b4fda48-8334-41f7-bfb3-0586cc4a392f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""6ec20008-3187-40f2-a40d-d3ce254ff91e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""68abfeed-b9a9-43da-a2e6-4292eebe4ce1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SilkSkill"",
                    ""type"": ""Button"",
                    ""id"": ""96170ed1-1b19-4007-a167-fd6c38a63877"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""ab4f81e7-204c-4c4e-a350-63bf625497a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DashAttack"",
                    ""type"": ""Button"",
                    ""id"": ""3fd37eea-2fa1-43f3-8e8d-6e3122f21429"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""487daf61-d777-40d8-a47e-b9ecac8ce8f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""InventoryMenu"",
                    ""type"": ""Button"",
                    ""id"": ""ae4f5979-6f24-43bf-a7c8-6f87ec47610d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Map"",
                    ""type"": ""Button"",
                    ""id"": ""40f5c2f4-9c6f-4b34-8605-b82b660f1f49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""0b713fc8-a3a1-4cc4-be35-07a9c2d2c484"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseRedTool"",
                    ""type"": ""Button"",
                    ""id"": ""b535c2d1-f312-41c7-acaa-2a5db5a497ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapToolRight"",
                    ""type"": ""Button"",
                    ""id"": ""c767b5cb-dcc7-4955-bb8f-9a8aa21b4d0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapToolLeft"",
                    ""type"": ""Button"",
                    ""id"": ""aa865740-c190-462e-aeba-f0f71e64c80e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c4146617-02bf-4148-ba2e-254a2020f656"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d47ea95c-0120-46ca-a0e5-8759eeb20907"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow keys"",
                    ""id"": ""ce6c97a0-f43d-4ccb-9903-4faa5dda52cc"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8f0b7137-8092-436d-b477-9e273c2ffe01"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""639608f0-0aa6-44ff-a5e8-7ba16d3b4140"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""64ec33b7-f354-40a4-892a-53c990a70630"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bdf1edd0-ff88-4ebd-a141-cd993b429814"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Leftstick"",
                    ""id"": ""3005def5-7b21-4bd0-be17-0309126ac48e"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6012ef66-d20c-4684-a3af-5090b04dc536"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""13c56135-923e-4bc5-a4a1-6b80a2df97f9"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""349dbbb5-5827-4581-8a18-de5c223265b5"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e5250769-abff-41e5-a1a4-dff7facd749a"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Dpad"",
                    ""id"": ""a71f6dfe-9011-46f9-b3b9-4f5cb699ce3c"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""483165c8-c039-492d-a199-dc0b5fa5f1bd"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""80d646d1-90a6-445c-88ad-11d16b2fad09"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""20413a41-89c2-431e-b718-7ef087080103"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f0df6447-5711-415d-9269-41b81957281d"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""856cc245-454a-4d8f-a3f8-6dc0d6d5f351"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d2c80ab-e4d9-4586-862a-7ec9a26f436f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7da01a81-a553-4c81-b4ff-8b852dac03cf"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SilkSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4860a8cc-0f5e-424f-9c94-a904a295a9d6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SilkSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5982380-0d84-4e9b-ae5c-9f2b72a47f9a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae4002ed-498e-439a-aef2-558ca88ea265"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""335fd868-d24e-4ae7-ab54-41a582065535"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b67a61d-c174-4846-9bc8-5c849940fd0e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab8b6e2d-90ff-42e3-bd13-325f38192d0d"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InventoryMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67f3d3c7-c0d4-4f2a-85ff-2ddecf314ed2"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InventoryMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87f1cee9-069f-468f-bc8c-f82cef058230"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6296404f-f2bd-4003-bdec-c466c983a11c"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""UseRedTool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46dd96e5-d0dc-4800-a9b8-3c4165aef6cb"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwapToolRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8e71f58-a4f5-4653-b635-4daa966071ee"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapToolLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2fa3982-5cfb-49ad-9abb-c4c5e9d4b50d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""UseRedTool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0d869d4-b2cc-44e3-949d-7ae2a79a4fc7"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""7cfeb17c-1abb-46d2-a090-6b41f9f1ed3b"",
            ""actions"": [
                {
                    ""name"": ""LeftTab"",
                    ""type"": ""Button"",
                    ""id"": ""e000e6a3-525c-44f1-8300-a1607b1c586f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightTab"",
                    ""type"": ""Button"",
                    ""id"": ""a49cd62a-9210-409c-96bc-16759e7d31d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OutOfMenu"",
                    ""type"": ""Button"",
                    ""id"": ""d89523a6-3a2e-44c5-9ee3-1db4ce931f01"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""3330b451-6e3f-4b1b-9f94-b87444ec8ea2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement2"",
                    ""type"": ""Value"",
                    ""id"": ""59bd1004-4dd0-43fa-b99d-ab198f7e7ec4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""26397dca-f01f-4e09-89ff-b8d20aaa79b3"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b29ec9bb-f8ee-4694-9eec-fec20a757f09"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""LeftTab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2ddf8f5-db12-4c7c-a16c-3a4e14d9df43"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RightTab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e54c8dd-87fd-4d55-9169-ac57178ce2eb"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OutOfMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99197068-5328-4d6e-99a7-d345e596e290"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OutOfMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow keys"",
                    ""id"": ""f8204f2d-3c31-4879-b77f-f34604ebf32b"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7cac69da-ec5d-4434-a4dc-ec17709f5729"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7953163d-ed5e-470c-ad09-8dde0efc7652"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f38e536c-4eec-4dbb-b433-4d932a29241f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6b42a90-4132-4578-9148-0b59b3049a23"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Leftstick"",
                    ""id"": ""af9b7d89-834c-4a11-9b9e-9a443aa3c836"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b37f4099-e84e-4c59-9caf-328c613081d9"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""02677c6b-511c-4dea-9261-60f5262a4e9f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""312b636a-4f08-490d-8c43-1dc6e96e1f9d"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""854dd171-2cb0-4585-af8e-40eca9faab6d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Dpad"",
                    ""id"": ""65a32b8a-80dd-498d-b3f7-f25a4e411cd7"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""48069b34-e56e-4f81-b610-ec6064e8df52"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6bcb8e0b-834e-4700-a2f5-98cf349015b5"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8404e340-9541-4c78-8e0f-c9f23caf71a4"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aac975fa-aae6-4a8a-97fc-2179f2f17c21"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""aafec359-2c27-4424-b3b7-74df8f7f6e87"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OutOfMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f383f0fe-f434-43e5-961d-da5c3b3763c1"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OutOfMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""359e0427-e46e-4c89-8b11-858de407160f"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OutOfMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""97a38cf7-7f02-47b8-a193-40458ec0d5c5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0fde6815-08a0-40e2-ad36-cb6183112840"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7cc7de27-d697-4732-8b72-9e2303567f6f"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow keys"",
                    ""id"": ""66e1c067-7a7a-41c4-bbfa-3327bf685224"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""29454d85-761e-4b60-8047-3e0079420640"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""78929b97-f897-4e07-9411-09074197c004"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""636015d8-ef45-49a8-9068-5fc489325db1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a1006357-9b4a-4430-93d5-6f32ee2823b0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Leftstick"",
                    ""id"": ""543f77c9-d749-48da-94c0-04e161cb0155"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cebbe142-0125-4a4e-82d9-a69daf96e083"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6d866ee8-2ac3-4b20-9900-e61970775eb6"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""938959fe-67c4-4666-a65b-4f91a977eff3"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4a4b1940-bcd1-48d5-810f-015242098feb"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_SilkSkill = m_Gameplay.FindAction("SilkSkill", throwIfNotFound: true);
        m_Gameplay_Parry = m_Gameplay.FindAction("Parry", throwIfNotFound: true);
        m_Gameplay_DashAttack = m_Gameplay.FindAction("DashAttack", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_InventoryMenu = m_Gameplay.FindAction("InventoryMenu", throwIfNotFound: true);
        m_Gameplay_Map = m_Gameplay.FindAction("Map", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_UseRedTool = m_Gameplay.FindAction("UseRedTool", throwIfNotFound: true);
        m_Gameplay_SwapToolRight = m_Gameplay.FindAction("SwapToolRight", throwIfNotFound: true);
        m_Gameplay_SwapToolLeft = m_Gameplay.FindAction("SwapToolLeft", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_LeftTab = m_Inventory.FindAction("LeftTab", throwIfNotFound: true);
        m_Inventory_RightTab = m_Inventory.FindAction("RightTab", throwIfNotFound: true);
        m_Inventory_OutOfMenu = m_Inventory.FindAction("OutOfMenu", throwIfNotFound: true);
        m_Inventory_Movement = m_Inventory.FindAction("Movement", throwIfNotFound: true);
        m_Inventory_Movement2 = m_Inventory.FindAction("Movement2", throwIfNotFound: true);
        m_Inventory_Zoom = m_Inventory.FindAction("Zoom", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_SilkSkill;
    private readonly InputAction m_Gameplay_Parry;
    private readonly InputAction m_Gameplay_DashAttack;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_InventoryMenu;
    private readonly InputAction m_Gameplay_Map;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_UseRedTool;
    private readonly InputAction m_Gameplay_SwapToolRight;
    private readonly InputAction m_Gameplay_SwapToolLeft;
    public struct GameplayActions
    {
        private @InputMaster m_Wrapper;
        public GameplayActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @SilkSkill => m_Wrapper.m_Gameplay_SilkSkill;
        public InputAction @Parry => m_Wrapper.m_Gameplay_Parry;
        public InputAction @DashAttack => m_Wrapper.m_Gameplay_DashAttack;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @InventoryMenu => m_Wrapper.m_Gameplay_InventoryMenu;
        public InputAction @Map => m_Wrapper.m_Gameplay_Map;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @UseRedTool => m_Wrapper.m_Gameplay_UseRedTool;
        public InputAction @SwapToolRight => m_Wrapper.m_Gameplay_SwapToolRight;
        public InputAction @SwapToolLeft => m_Wrapper.m_Gameplay_SwapToolLeft;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @SilkSkill.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSilkSkill;
                @SilkSkill.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSilkSkill;
                @SilkSkill.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSilkSkill;
                @Parry.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnParry;
                @Parry.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnParry;
                @Parry.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnParry;
                @DashAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashAttack;
                @DashAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashAttack;
                @DashAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashAttack;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @InventoryMenu.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInventoryMenu;
                @InventoryMenu.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInventoryMenu;
                @InventoryMenu.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInventoryMenu;
                @Map.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @Map.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @Map.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @UseRedTool.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseRedTool;
                @UseRedTool.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseRedTool;
                @UseRedTool.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseRedTool;
                @SwapToolRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolRight;
                @SwapToolRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolRight;
                @SwapToolRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolRight;
                @SwapToolLeft.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolLeft;
                @SwapToolLeft.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolLeft;
                @SwapToolLeft.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwapToolLeft;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @SilkSkill.started += instance.OnSilkSkill;
                @SilkSkill.performed += instance.OnSilkSkill;
                @SilkSkill.canceled += instance.OnSilkSkill;
                @Parry.started += instance.OnParry;
                @Parry.performed += instance.OnParry;
                @Parry.canceled += instance.OnParry;
                @DashAttack.started += instance.OnDashAttack;
                @DashAttack.performed += instance.OnDashAttack;
                @DashAttack.canceled += instance.OnDashAttack;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @InventoryMenu.started += instance.OnInventoryMenu;
                @InventoryMenu.performed += instance.OnInventoryMenu;
                @InventoryMenu.canceled += instance.OnInventoryMenu;
                @Map.started += instance.OnMap;
                @Map.performed += instance.OnMap;
                @Map.canceled += instance.OnMap;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @UseRedTool.started += instance.OnUseRedTool;
                @UseRedTool.performed += instance.OnUseRedTool;
                @UseRedTool.canceled += instance.OnUseRedTool;
                @SwapToolRight.started += instance.OnSwapToolRight;
                @SwapToolRight.performed += instance.OnSwapToolRight;
                @SwapToolRight.canceled += instance.OnSwapToolRight;
                @SwapToolLeft.started += instance.OnSwapToolLeft;
                @SwapToolLeft.performed += instance.OnSwapToolLeft;
                @SwapToolLeft.canceled += instance.OnSwapToolLeft;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_LeftTab;
    private readonly InputAction m_Inventory_RightTab;
    private readonly InputAction m_Inventory_OutOfMenu;
    private readonly InputAction m_Inventory_Movement;
    private readonly InputAction m_Inventory_Movement2;
    private readonly InputAction m_Inventory_Zoom;
    public struct InventoryActions
    {
        private @InputMaster m_Wrapper;
        public InventoryActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftTab => m_Wrapper.m_Inventory_LeftTab;
        public InputAction @RightTab => m_Wrapper.m_Inventory_RightTab;
        public InputAction @OutOfMenu => m_Wrapper.m_Inventory_OutOfMenu;
        public InputAction @Movement => m_Wrapper.m_Inventory_Movement;
        public InputAction @Movement2 => m_Wrapper.m_Inventory_Movement2;
        public InputAction @Zoom => m_Wrapper.m_Inventory_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @LeftTab.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnLeftTab;
                @LeftTab.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnLeftTab;
                @LeftTab.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnLeftTab;
                @RightTab.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRightTab;
                @RightTab.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRightTab;
                @RightTab.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRightTab;
                @OutOfMenu.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnOutOfMenu;
                @OutOfMenu.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnOutOfMenu;
                @OutOfMenu.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnOutOfMenu;
                @Movement.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement;
                @Movement2.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement2;
                @Movement2.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement2;
                @Movement2.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMovement2;
                @Zoom.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftTab.started += instance.OnLeftTab;
                @LeftTab.performed += instance.OnLeftTab;
                @LeftTab.canceled += instance.OnLeftTab;
                @RightTab.started += instance.OnRightTab;
                @RightTab.performed += instance.OnRightTab;
                @RightTab.canceled += instance.OnRightTab;
                @OutOfMenu.started += instance.OnOutOfMenu;
                @OutOfMenu.performed += instance.OnOutOfMenu;
                @OutOfMenu.canceled += instance.OnOutOfMenu;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Movement2.started += instance.OnMovement2;
                @Movement2.performed += instance.OnMovement2;
                @Movement2.canceled += instance.OnMovement2;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnSilkSkill(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
        void OnDashAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInventoryMenu(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnUseRedTool(InputAction.CallbackContext context);
        void OnSwapToolRight(InputAction.CallbackContext context);
        void OnSwapToolLeft(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnLeftTab(InputAction.CallbackContext context);
        void OnRightTab(InputAction.CallbackContext context);
        void OnOutOfMenu(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnMovement2(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
