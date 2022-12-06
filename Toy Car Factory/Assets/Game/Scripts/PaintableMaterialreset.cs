using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableMaterialreset : MonoBehaviour
{

    public Material ogMat;
    public Renderer meshRenderer;

    void OnEnable() => meshRenderer.material = ogMat;
}
