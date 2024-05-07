using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
///<summary>
///
///</summary>
public class UIInput : Singleton<UIInput>
{
    InputSystemUIInputModule UIInputModule;
    [SerializeField] PlayerInput playerInput;
    protected override void Awake()
    {
        base.Awake();
        UIInputModule= GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }
    public void SelectUI(Selectable UiObject)
    {
        UiObject.Select();
        UiObject.OnSelect(null);
        UIInputModule.enabled = true;
    }
    public void DisableAllUIInputs()
    {
        playerInput.DisAbleAllInputs();
        UIInputModule.enabled= false;
    }
}
