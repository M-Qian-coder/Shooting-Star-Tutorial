using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class StatsBar : MonoBehaviour
{
    Canvas canvas;
    float currentFillAmount;
    protected float targetFillAmount;
    float t;
    WaitForSeconds waitForDelayFill;
    [SerializeField]Image fillImageBack;
    [SerializeField]Image fillImageFront;
    [SerializeField] float fillSpeed = 0.1f;
    [SerializeField] bool delayFill=true;
    [SerializeField] float fillDelay = 0.1f;
    float previousFillAmount;
    Coroutine bufferedFillingCoroutine;
    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount= currentValue/maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = targetFillAmount;
        fillImageFront.fillAmount = targetFillAmount;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="currentValue">��ǰ״̬��ֵ</param>
    /// <param name="maxValue">״̬�����ֵ</param>
    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue; 
        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        if (currentFillAmount>targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
           
            bufferedFillingCoroutine=StartCoroutine(nameof(BufferedFillingCoroutine), fillImageBack);
        }
        else if(currentFillAmount<targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine =StartCoroutine(nameof(BufferedFillingCoroutine), fillImageFront);
        }
    }
    /// <summary>
    /// ��仺��Э��
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFill)
        {
            yield return waitForDelayFill;
        }
        previousFillAmount= currentFillAmount;
        t = 0f;
        while (t<1f) 
        {
            t+= Time.deltaTime*fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount=currentFillAmount;
            yield return null;
         }
        
    }
}
