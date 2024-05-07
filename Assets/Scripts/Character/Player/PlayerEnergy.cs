using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField]EnegyBar enegyBar;
    public const int MAX = 100;
    public const int PERCENT = 1;
    [SerializeField]int enegy;
    [SerializeField]float overdriveInterval = 0.1f;
    bool available = true;

    WaitForSeconds waitForOverdriveInterval;
    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }
    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval=new WaitForSeconds(overdriveInterval);
    }
    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }
    private void Start()
    {
        enegyBar.Initialize(enegy, MAX);
        Obtain(MAX);
    }
    public void Obtain(int value)
    {
        if (enegy == MAX || !available||!gameObject.activeSelf) return;
        if (enegy == MAX) return;
        enegy=Mathf.Clamp(enegy + value, 0, MAX);
        enegyBar.UpdateStats(enegy,MAX);
    }
    public void Use(int Value)
    {
        
        enegy=Mathf.Clamp(enegy-Value, 0, MAX);
        enegyBar.UpdateStats(enegy,MAX);
        if (enegy==0&&!available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }
    public bool IsEnough(int value)
    {
        return enegy >= value;
    }
    void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf&&enegy>0)
        {
            yield return waitForOverdriveInterval;
            Use(PERCENT);
        }
    }
}
