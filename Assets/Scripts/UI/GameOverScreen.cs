using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Canvas HUDCanvas;
    [SerializeField] AudioData confirmGameOverSound;
    Animator animator;
    Canvas canvas;
    int exitStateID = Animator.StringToHash("GameOverScreenExit");
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
        canvas.enabled = false;
        animator.enabled = false;
    }
    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        input.onConfirmGameOver += OnConFirmGameOver;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConFirmGameOver;
    }
    void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisAbleAllInputs();
    }
    void OnConFirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        input.DisAbleAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();
    }
    public void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
}
