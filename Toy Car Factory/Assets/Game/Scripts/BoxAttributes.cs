using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAttributes : MonoBehaviour
{
    int price;
    public int GetPrice() => price;
    public void SetPrice(int price) => this.price = price;
}
