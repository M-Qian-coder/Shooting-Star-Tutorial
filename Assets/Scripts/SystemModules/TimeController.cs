using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0f,1f)] float bulletTimeScale = 0.1f;
    float timeScaleBeforePause; 

    float defaultFixDeltaTime;
    float t;

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
        
    }
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;
        
    }
    protected override void Awake()
    {
        base.Awake();
        defaultFixDeltaTime = Time.fixedDeltaTime;
    }
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
        
    }
    public void BulletTime(float inDuration,float outDuration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowInAndOutCoroutine(inDuration,outDuration));

    }
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }
    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }
    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuratin)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuratin));
    }
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            if(GameManager.GameState!=GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixDeltaTime * Time.timeScale;
            }
           
            yield return null;
        }
    }
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;
        while (t<1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixDeltaTime * Time.timeScale;
            }
                yield return null;
        }
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    
}
