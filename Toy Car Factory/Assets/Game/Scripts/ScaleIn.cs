using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    Vector3 targetScale;

    public float scaleSpeed;
    float lerpTime;
    bool scale;

    /*
    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
        lerpTime = 0f;
    }
    */

    void OnEnable()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
        lerpTime = 0f;
        scale = true;
    }

    void Update()
    {
        if (!scale)
            return;

        lerpTime += Time.deltaTime * scaleSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTime);
        if (Vector3.Distance(transform.localScale, targetScale) < 0.001f)
            scale = false;
    }
}
