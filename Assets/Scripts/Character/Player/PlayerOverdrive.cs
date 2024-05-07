using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///
///</summary>
public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };
    [SerializeField] GameObject triggerVFX;
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;
    [SerializeField] AudioData onSFX;
    [SerializeField] AudioData offSFX;
    private void Awake()
    {
        on += On;
        off += Off;
    }
    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }
    void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }

}
