using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToNothing : MonoBehaviour
{

    public float scaleSpeed;
    float lerpTime;

    bool scale;

    void OnEnable()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;
        lerpTime = 0f;
        scale = true;
    }

    void Update()
    {
        if (!scale)
            return;

        lerpTime += Time.deltaTime * scaleSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, lerpTime);
        if (Vector3.Distance(transform.localScale, Vector3.zero) < 0.01f)
        {
            scale = false;
            if (this.tag == "purchaseSlot")
                Invoke("SelfDestruct", 1f);
            else
                this.enabled = false;
        }
    }

    void SelfDestruct() => Destroy(gameObject);
}
