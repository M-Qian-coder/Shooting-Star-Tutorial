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
    private void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
