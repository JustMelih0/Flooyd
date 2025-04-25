using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterStateMachine))]

public class CharacterInputController : MonoBehaviour
{

    private CharacterStateMachine machine;

    private PlayerInput playerInput;

    private InputType? bufferedInputType = null;
    private ClickType bufferedClickType;
    private float bufferStartTime;
    private const float bufferDuration = 0.25f;





    private void Awake()
    {
        machine = GetComponent<CharacterStateMachine>();
        playerInput = GetComponent<PlayerInput>();

        machine.StateChangedEvent += OnStateChanged;
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

        machine.StateChangedEvent -= OnStateChanged;
    }

    void OpenInputController()
    {
        playerInput.enabled = true;
    }
    void CloseInputController()
    {
        playerInput.enabled = false;
    }
    private void BufferInput(InputType type, ClickType click)
    {
        bufferedInputType = type;
        bufferedClickType = click;
        bufferStartTime = Time.time;
    }
    private void TryInput(InputType inputType, ClickType clickType)
    {
        if (machine.canTransitionState)
        {
            SendInputToCorrectState(inputType, clickType);
        }
        else
        {
            BufferInput(inputType, clickType);
        }
    }
    private void Update()
    {
        if (bufferedInputType.HasValue)
        {
            if (Time.time - bufferStartTime > bufferDuration)
            {
                bufferedInputType = null;
                return;
            }

            if (machine.canTransitionState)
            {
                SendInputToCorrectState(bufferedInputType.Value, bufferedClickType);
                bufferedInputType = null;
            }
        }
    }
    private void SendInputToCorrectState(InputType inputType, ClickType clickType)
    {
        switch (inputType)
        {
            case InputType.AttackInput:
                machine.character_AttackState.InputRequest(inputType, clickType);
                break;
            case InputType.DashInput:
                machine.character_DashState.InputRequest(inputType, clickType);
                break;
            case InputType.BlockInput:
                machine.character_ParryState.InputRequest(inputType, clickType);
                break;
            case InputType.JumpInput:
                machine.character_LocomotionState.InputRequest(inputType, clickType);
                break;
            case InputType.HorizontalInput:
                machine.character_LocomotionState.InputRequest(inputType, clickType);
                break;

        }
    }
    public void OnStateChanged() 
    {
        if(machine.currentState is not Character_LocomotionState)
        bufferedInputType = null;
    }
    public void HorizontalInput(InputAction.CallbackContext context)
    {
        //characterFeatures.horizontalInput = context.ReadValue<float>();
        if (context.started)
        {
            TryInput(InputType.HorizontalInput, ClickType.started);
        }
    }
    public void AttackInput(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            TryInput(InputType.AttackInput, ClickType.started);
        }

    }
    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryInput(InputType.DashInput, ClickType.started);
        }
    }
    public void BlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryInput(InputType.BlockInput, ClickType.started);
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryInput(InputType.JumpInput, ClickType.started);
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
