using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class ScoringUIController : MonoBehaviour
{
    [Header("--BACKGROUND--")]
    [SerializeField]Image background;
    [SerializeField]Sprite[] backgroundImages;
    [Header("--SCORING SCREEN--")]
    [SerializeField] Canvas scoringSceenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;
    private void Start()
    {
        ShowRandomBackGround();
        ShowScoringScreen();

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        GameManager.GameState = GameState.Scoring;
    }
    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }
    void ShowRandomBackGround()
    {
        background.sprite=backgroundImages[Random.Range(0, backgroundImages.Length)];
    }
    void ShowScoringScreen()
    {
        scoringSceenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void OnButtonMainMenuClicked()
    {
        scoringSceenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
