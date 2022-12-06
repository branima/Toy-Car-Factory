using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScaleIn : MonoBehaviour
{

    Vector3 targetScale;

    public float scaleSpeed;
    float lerpTime;

    public FactoryLogic factoryScript;

    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
        lerpTime = 0f;
    }

    void Update()
    {
        lerpTime += Time.deltaTime * scaleSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTime);
        if (Vector3.Distance(transform.localScale, targetScale) < 0.001f)
        {
            factoryScript.enabled = true;
            this.enabled = false;
        }
    }
}
