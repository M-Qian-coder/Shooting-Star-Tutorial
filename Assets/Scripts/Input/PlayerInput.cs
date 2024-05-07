using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputActions;

[CreateAssetMenu(menuName = "Player Input")]
///<summary>
///
///</summary>

public class PlayerInput : ScriptableObject, InputActions.IGameOverScreenActions, InputActions.IGamePlayActions, InputActions. IPauseMenuActions
{
    public event UnityAction<Vector2> onMove=delegate { };
    public event UnityAction onStopMove=delegate { };
    public event UnityAction onFire=delegate { };
    public event UnityAction onStopFire=delegate { };
    public event UnityAction onDodge=delegate { };
    public event UnityAction onOverDrive= delegate { };
    public event UnityAction onPause=delegate { };
    public event UnityAction onUnPause = delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction onConfirmGameOver=delegate { };
    InputActions inputActions;
    void OnEnable()
    {
        inputActions=new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisAbleAllInputs();
    }
    /// <summary>
    /// 启用GamePaly动作表，隐藏鼠标，锁定鼠标
    /// </summary>
    public void EnableGamePlayInput()
    {
        SwitchActionMap(inputActions.GamePlay,false);
    }
    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu, true);
    }
    public void EnableGameOverScreenInput()
    {
        SwitchActionMap(inputActions.GameOverScreen, false);
    }

    void  SwitchActionMap(InputActionMap actionsMap,bool isUIinput)
    {
        inputActions.Disable();
        actionsMap.Enable();
        if(isUIinput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    /// <summary>
    /// 关闭所有动作表
    /// </summary>
    public void DisAbleAllInputs()
    {
        inputActions.Disable();
    }
    /// <summary>
    /// 实现接口OnMove函数，通过回调信息调用事件
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(onMove!=null)
                onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    /// <summary>
    /// 实现接口OnFire函数，通过回调信息调用事件
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onFire.Invoke();
        }
        if(context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverDrive(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onOverDrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onUnPause.Invoke();
        }
    }
    public void SwitchToDynamicUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    }
    public void SwitchToFixUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
