using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class ResetPaintableTexture : MonoBehaviour
{

    public P3dPaintableTexture paintableTexture;

    void OnEnable() => paintableTexture.Clear();
}
