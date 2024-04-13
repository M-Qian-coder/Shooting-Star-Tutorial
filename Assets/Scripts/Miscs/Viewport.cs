using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Viewport : Singleton<Viewport>
{
    float minx;
    float miny;
    float maxx;
    float maxy;
  
    private void Start()
    {
        Camera mainCamera= Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector2(0f, 0f));
        minx= bottomLeft.x;
        miny= bottomLeft.y;
        Vector2 TopRight = mainCamera.ViewportToWorldPoint(new Vector2(1f, 1f));
        maxx= TopRight.x;
        maxy= TopRight.y;
        
    }
    public Vector3 PlayerMoveablePosition(Vector3 palyerPosition,float paddingx,float paddingy)
    {
        //Vector3 position = Vector3.zero;
        palyerPosition.x= Mathf.Clamp(palyerPosition.x, minx+ paddingx, maxx- paddingx);
        palyerPosition.y= Mathf.Clamp(palyerPosition.y, miny+ paddingy, maxy- paddingy);
        return palyerPosition;
    }

}
