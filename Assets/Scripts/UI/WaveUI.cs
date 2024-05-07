using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class WaveUI : MonoBehaviour
{
    Text waveText;
     void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        waveText.text = "- Wave " + EnemyManager.Instance.WaveNumber + " -";
    }
}
