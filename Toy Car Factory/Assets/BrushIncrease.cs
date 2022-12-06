using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class BrushIncrease : MonoBehaviour
{

    public P3dPaintSphere brushScript;
    float ogRadius;
    float sizedDownRadius;

    void Start()
    {
        ogRadius = brushScript.Radius;
        sizedDownRadius = brushScript.Radius / 2f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            brushScript.Radius = ogRadius;
            Debug.Log("Q");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            brushScript.Radius = sizedDownRadius;
            Debug.Log("E");
        }
    }
}
