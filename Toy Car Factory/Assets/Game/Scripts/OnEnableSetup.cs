using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSetup : MonoBehaviour
{
    public PaintingLogic paintingScript;
    void OnEnable() => paintingScript.EnableChasisSelection();
}
