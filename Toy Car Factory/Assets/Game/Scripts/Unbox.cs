using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unbox : MonoBehaviour
{

    bool scale;

    public Animator openingAnim;
    public GameObject arrow;

    public ParticleSystem confettiParticles;

    void Update()
    {
        if (!scale)
        {
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if (hit.transform == transform)
                {
                    scale = true;
                    arrow.SetActive(false);
                    openingAnim.enabled = true;
                    confettiParticles.Play();
                }
            }
        }
    }

    void BeginUnboxing()
    {
        
        Transform boxContent = transform.GetChild(transform.childCount - 1);
        boxContent.parent = null;
        boxContent.gameObject.SetActive(true);

        ScaleToNothing stn = gameObject.AddComponent<ScaleToNothing>();
        stn.scaleSpeed = 0.05f;
    }
}
