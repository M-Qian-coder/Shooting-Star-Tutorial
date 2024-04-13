using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
///<summary>
///
///</summary>

public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions
{
    public event UnityAction<Vector2> onMove=delegate { };
    public event UnityAction onStopMove=delegate { };
    public event UnityAction onFire=delegate { };
    public event UnityAction onStopFire=delegate { };
    InputActions inputActions;
    void OnEnable()
    {
        inputActions=new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisAbleAllInputs();
    }
    public void EnableGamePlayInput()
    {
        inputActions.GamePlay.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void DisAbleAllInputs()
    {
        inputActions.GamePlay.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Performed)
        {
            if(onMove!=null)
                onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.phase==InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Performed)
        {
            onFire.Invoke();
        }
        if(context.phase==InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }
}
