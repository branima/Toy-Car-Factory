using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScaleIn : MonoBehaviour
{
    public FactoryLogic factoryScript;
    public void EnableFactoryScript() => factoryScript.enabled = true;
}
