using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class BrushIncrease : MonoBehaviour
{

    public P3dPaintSphere brushScript;
    float ogRadius;
    float sizedDownRadius;

    void Awake()
    {
        ogRadius = brushScript.Radius;
        sizedDownRadius = brushScript.Radius / 2f;
    }

    public void BigBrush() => brushScript.Radius = ogRadius;
    public void SmallBrush() => brushScript.Radius = sizedDownRadius;
}
