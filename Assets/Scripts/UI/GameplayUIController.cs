using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class GameplayUIController : MonoBehaviour
{
    [Header("----PLAYER INPUT----")]
    [SerializeField] PlayerInput playerInput;
    [Header("----Canvas----")]
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Canvas menusCanvas;
    [Header("----Button----")]
    [SerializeField] Button resumButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mianmenuButton;
    [Header("----AUDIO DATA----")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    int buttonPressdParameterID = Animator.StringToHash("Pressed");
    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += UnPause;
        ButtonPressedBehavior.buttonFunctionTable.Add(resumButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mianmenuButton.gameObject.name, OnMainmenubuttonClick);
    }
    private void OnDisable()
    {   
        
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= UnPause;
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }
    void Pause()
    {
        TimeController.Instance.Pause();
        playerInput.SwitchToDynamicUpdateMode();
        GameManager.GameState=GameState.Paused;
        hudCanvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        UIInput.Instance.SelectUI(resumButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    public void UnPause()
    {
        resumButton.Select();
        resumButton.animator.SetTrigger(buttonPressdParameterID);
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }
    void OnResumeButtonClick()
    {
        GameManager.GameState = GameState.Playing;
        playerInput.SwitchToFixUpdateMode();
        TimeController.Instance.UnPause();
        menusCanvas.enabled = false;
        hudCanvas.enabled = true;
        playerInput.EnableGamePlayInput();
    }
    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }
    void OnMainmenubuttonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
