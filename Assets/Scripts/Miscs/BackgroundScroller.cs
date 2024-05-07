using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]Vector2 scrollVelocity;
    Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    IEnumerator Start()
    {
        while(GameManager.GameState!=GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }
    private void Update()
    {
      
    }
}
