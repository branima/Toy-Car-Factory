using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttributes : MonoBehaviour
{
    public int price;
    FactoryLogic originFactory;

    public int GetPrice() => price;
    public void SetOriginFactory(FactoryLogic originFactory) => this.originFactory = originFactory;
    public FactoryLogic GetOriginFactory() => originFactory;
}
