using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
            gameObject.SetActive(false);
    }
}
