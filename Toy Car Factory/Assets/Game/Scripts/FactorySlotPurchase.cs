using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactorySlotPurchase : MonoBehaviour
{


    public int price;
    TextMeshProUGUI priceText;

    void Start()
    {
        priceText = GetComponentInChildren<TextMeshProUGUI>();

        if (price < 1000)
            priceText.text = "$" + price;
        else if (price < 1000000)
            priceText.text = "$" + (price * 1f / 1000).ToString("n1") + "K";
        else
            priceText.text = "$" + (price * 1f / 1000000).ToString("n1") + "M";
    }

    public int GetPrice() => price;
}
