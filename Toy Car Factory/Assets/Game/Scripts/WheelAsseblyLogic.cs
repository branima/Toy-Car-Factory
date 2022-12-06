using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAsseblyLogic : MonoBehaviour
{

    public Animator pressAnim;

    void OnTriggerEnter(Collider other)
    {
        pressAnim.SetTrigger("pressTrigger");
        for (int i = 0; i < 4; i++)
            other.transform.GetChild(i).gameObject.SetActive(true);
    }
}
