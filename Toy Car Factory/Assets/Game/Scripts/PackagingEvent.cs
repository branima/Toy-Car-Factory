using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagingEvent : MonoBehaviour
{

    public BoxPackagingLogic boxPackageScript;

    void Pack() => boxPackageScript.Pack();
}
