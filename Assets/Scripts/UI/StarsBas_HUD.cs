using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class StarsBas_HUD : StatsBar
{
    [SerializeField]Text percentText;
    void SetPercentText()
    {
        percentText.text = Mathf.RoundToInt(targetFillAmount*100f)+"%";
    }
    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }
    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}
