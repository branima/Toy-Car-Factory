using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttributes : MonoBehaviour
{
    public int price;
    FactoryLogic originFactory;
    MiniTrackLogic miniTrackScript;

    public int GetPrice() => price;
    public void SetOriginFactory(FactoryLogic originFactory, MiniTrackLogic miniTrackScript)
    {
        this.originFactory = originFactory;
        this.miniTrackScript = miniTrackScript;
    }
    public FactoryLogic GetOriginFactory() => originFactory;
    public MiniTrackLogic GetMiniTrackScript() => miniTrackScript;
}
