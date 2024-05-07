using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///<summary>
///
///</summary>
public class ScoreDisplay : MonoBehaviour
{
    static Text scoreText;
    void Awake()
    {
        scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }
    public static void updateText(int score)
    {
        scoreText.text=score.ToString();
    }
    public static void ScaleText(Vector3 targetScale)
    {
        scoreText.rectTransform.localScale = targetScale;
    }
}
