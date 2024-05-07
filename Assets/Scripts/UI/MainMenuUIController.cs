using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class MainMenuUIController : MonoBehaviour
{
    [Header("--CANVAS--")]
    [SerializeField] Canvas mianMenuCanvas;
    [Header("--button--")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOption;
    [SerializeField] Button buttonQuit;
    private void Start()
    {
        Time.timeScale = 1.0f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    private void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOption.gameObject.name, OnButtonOptionClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
     
    }
    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }
    void OnButtonStartClicked()
    {
        SceneLoader.Instance.LoadGameplayScene();
    }
    void OnButtonOptionClicked()
    {
        UIInput.Instance.SelectUI(buttonStart);
    }
    void OnButtonQuitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
