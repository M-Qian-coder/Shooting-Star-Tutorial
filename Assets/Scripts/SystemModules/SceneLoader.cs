using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] Image transitionimage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;

    const string GAMEPLAY = "Gameplay";
    const string MAINMENU = "MainMenu";
    const string SCORING = "Scoring";
    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation=SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        transitionimage.gameObject.SetActive(true);
        while(color.a<1f)
        {
            color.a=Mathf.Clamp01( color.a += Time.unscaledDeltaTime / fadeTime);
            transitionimage.color = color;
            yield return null;
        }
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        loadingOperation.allowSceneActivation = true;
        while (color.a >0f)
        {
            color.a = Mathf.Clamp01(color.a -= Time.unscaledDeltaTime / fadeTime);
            transitionimage.color = color;
            yield return null;

        }
        transitionimage.gameObject.SetActive(false);
    }

    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(LoadingCoroutine), GAMEPLAY);
    }
    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(LoadingCoroutine), MAINMENU);
    }
    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(LoadingCoroutine), SCORING);
    }
}

