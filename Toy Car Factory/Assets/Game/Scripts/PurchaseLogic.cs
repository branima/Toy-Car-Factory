using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseLogic : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject mainGameplayScene;
    public GameObject paintingScene;
    public GameObject gameplayCanvas;
    public GameObject paintingCanvas;

    FactoryLogic newFactoryScript;

    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.transform.name.Contains("FactorySlotPurchase"))
            {
                int price = hit.transform.GetComponent<FactorySlotPurchase>().GetPrice();
                if (gameManager.GetMoney() >= price)
                {
                    ///MAIN GAMEPLAY PAUSE
                    ///SCENE SWITCH

                    mainGameplayScene.SetActive(false);
                    gameplayCanvas.SetActive(false);
                    paintingScene.SetActive(true);
                    paintingCanvas.SetActive(true);

                    ///SCENE SWITCH BACK
                    ///MAIN GAMEPLAY RESUME
                    gameManager.AddMoney(-price);
                    Transform factory = hit.transform.GetChild(hit.transform.childCount - 1);
                    factory.parent = mainGameplayScene.transform;
                    factory.gameObject.SetActive(true);
                    newFactoryScript = factory.GetComponent<FactoryLogic>();
                    PulsingScaleAnim psa = hit.transform.gameObject.GetComponent<PulsingScaleAnim>();
                    psa.Purchased();
                    ScaleToNothing stn = hit.transform.gameObject.AddComponent<ScaleToNothing>();
                    stn.scaleSpeed = 0.25f;
                }
            }
            else if (hit.transform.tag.Contains("newZoneDoors"))
            {
                int price = gameManager.GetNextLevelPrice();
                if (gameManager.GetMoney() >= price)
                    gameManager.NextLevel();
            }
        }
    }

    public FactoryLogic GetLatestFactory() => newFactoryScript;
}
