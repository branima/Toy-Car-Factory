using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTruck : MonoBehaviour
{
    public SellingBoxLogic sellingBoxScript;
    public void EmptyTruck() => sellingBoxScript.ClearTruck();
}
