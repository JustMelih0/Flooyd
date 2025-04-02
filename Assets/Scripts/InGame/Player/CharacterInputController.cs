using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterStateMachine))]

public class CharacterInputController : MonoBehaviour
{

    private CharacterStateMachine machine;

    private PlayerInput playerInput;
    private void Awake()
    {
        machine = GetComponent<CharacterStateMachine>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        //GameManager.OnGamePaused += CloseInputController;
        //GameManager.OnGameResumed += OpenInputController;

    }
    private void OnDisable()
    {
        // GameManager.OnGamePaused -= CloseInputController;
        //GameManager.OnGameResumed -= OpenInputController;

    }
    void OpenInputController()
    {
        playerInput.enabled = true;
    }
    void CloseInputController()
    {
        playerInput.enabled = false;
    }

    public void HorizontalInput(InputAction.CallbackContext context)
    {
        //characterFeatures.horizontalInput = context.ReadValue<float>();
    }
    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //character.attackState.InputRequest(InputType.AttackInput, ClickType.started);
        }
        else if (context.canceled)
        {
            //character.attackState.InputRequest(InputType.AttackInput, ClickType.canceled);
        }

    }
    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            machine.character_DashState.InputRequest(InputType.DashInput, ClickType.started);
        }
    }
    public void BlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //character.blockState.InputRequest(InputType.BlockInput, ClickType.started);
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            machine.character_LocomotionState.InputRequest(InputType.JumpInput, ClickType.started);
        }
    }
    public void PotDrinkInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //character.potDrinkState.InputRequest(InputType.PotDrinkInput, ClickType.started);
        }
    }
    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //characterFeatures.InteractRequest();
        }
    }




    [System.Serializable]
    public enum InputType
    {
        AttackInput,
        HorizontalInput,
        JumpInput,
        DashInput,
        BlockInput,
        PotDrinkInput
    }

    [System.Serializable]
    public enum ClickType
    {
        started,
        performed,
        canceled,
    }
}
