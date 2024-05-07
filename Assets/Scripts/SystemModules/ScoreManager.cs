using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public int Score => score;
    int score;
    int currentScore;
    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.updateText(score);
    }

    public void AddScore(int scorepoint)
    {
        currentScore += scorepoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score<currentScore)
        {
            score += 1;
            ScoreDisplay.updateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }
}
