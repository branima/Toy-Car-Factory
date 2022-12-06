using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingScaleAnim : MonoBehaviour
{

    public GameManager gameManager;
    public FactorySlotPurchase factorySlotScript;
    int price;

    public Animator tileAnimator;

    public GameObject tutorialArrow;

    // Start is called before the first frame update
    void Start() => price = factorySlotScript.GetPrice();

    // Update is called once per frame
    void Update()
    {
        if (price <= gameManager.GetMoney())
        {
            if (!tileAnimator.GetBool("pulse"))
                tileAnimator.SetBool("pulse", true);
        }
        else
        {
            if (tileAnimator.GetBool("pulse"))
                tileAnimator.SetBool("pulse", false);
        }

    }

    public void Purchased()
    {
        tileAnimator.SetBool("pulse", false);
        if (tutorialArrow != null && tutorialArrow.activeSelf)
            tutorialArrow.SetActive(false);
        this.enabled = false;
    }

}
