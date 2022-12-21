using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellingBoxLogic : MonoBehaviour
{

    public GameManager gameManager;

    public int maxCapacity;
    int currCapacity;
    public TextMeshProUGUI capacityText;

    public MainTrackLogic mainTrackScript;

    int collectedMoney;
    public TextMeshProUGUI collectedMoneyText;

    bool fullCapVisualsOn;
    public Animator textAnimator;
    public Material normalCapTextMat;
    public Material fullCapTextMat;

    public Animator truckAnim;

    public GameObject tutorialArrow;

    int lastCarIdx;
    public List<GameObject> packedCars;

    int lastUpdate;
    public Transform truckUpgrades;
    List<Transform> packedCarsPositions;

    void Start()
    {
        capacityText.text = currCapacity + "/" + maxCapacity;
        collectedMoneyText.text = "$0";

        fullCapVisualsOn = false;
        capacityText.fontSharedMaterial = normalCapTextMat;
        lastCarIdx = -1;

        lastUpdate = 0;
        packedCarsPositions = new List<Transform>();
        truckUpgrades.GetChild(lastUpdate).gameObject.SetActive(true);
        foreach (Transform pos in truckUpgrades.GetChild(lastUpdate))
            packedCarsPositions.Add(pos);
    }

    void OnTriggerEnter(Collider other)
    {
        ScaleToNothing stn = other.GetComponent<ScaleToNothing>();
        if (stn == null)
        {
            mainTrackScript.RemoveCarFromList(other.transform);
            if (currCapacity < maxCapacity)
            {
                packedCars.Add(other.gameObject);
                TruckFillingVisuals(other.transform);
                if (tutorialArrow != null && !tutorialArrow.activeSelf)
                    tutorialArrow.SetActive(true);

                capacityText.text = ++currCapacity + "/" + maxCapacity;

                collectedMoney += other.GetComponent<CarAttributes>().GetPrice();
                collectedMoneyText.text = collectedMoney.ToString();
                if (collectedMoney < 1000)
                    collectedMoneyText.text = "$" + collectedMoney;
                else if (collectedMoney < 1000000)
                    collectedMoneyText.text = "$" + (collectedMoney * 1f / 1000).ToString("n1") + "K";
                else
                    collectedMoneyText.text = "$" + (collectedMoney * 1f / 1000000).ToString("n1") + "M";

                if (currCapacity == maxCapacity && !fullCapVisualsOn)
                {
                    fullCapVisualsOn = true;
                    textAnimator.SetBool("full", true);
                    capacityText.fontSharedMaterial = fullCapTextMat;
                }
            }
            else
            {
                ReturnToPool(other.transform);
                other.gameObject.SetActive(false);
            }
        }
    }

    void UpgradeTruck()
    {
        lastUpdate++;
        truckUpgrades.GetChild(lastUpdate).gameObject.SetActive(true);
        foreach (Transform pos in truckUpgrades.GetChild(lastUpdate))
            packedCarsPositions.Add(pos);
    }

    public void ReturnToPool(Transform car)
    {
        mainTrackScript.RemoveCarFromList(car);
        car.GetComponent<CarAttributes>().GetOriginFactory().EnqueueCar(car);
    }

    public void Sell()
    {
        if (tutorialArrow != null && tutorialArrow.activeSelf)
        {
            tutorialArrow.SetActive(false);
            tutorialArrow = null;
        }

        truckAnim.SetTrigger("drive");
        gameManager.AddMoney(collectedMoney);
        collectedMoney = 0;
        collectedMoneyText.text = "$" + collectedMoney.ToString();

        lastCarIdx = -1;
        foreach (GameObject item in packedCars)
        {
            item.transform.parent = null;
            item.GetComponent<CarAttributes>().GetOriginFactory().EnqueueCar(item.transform);
            item.SetActive(false);
        }
        packedCars.Clear();

        currCapacity = 0;
        capacityText.text = currCapacity + "/" + maxCapacity;

        fullCapVisualsOn = false;
        textAnimator.SetBool("full", false);
        capacityText.fontSharedMaterial = normalCapTextMat;
    }

    public int GetCurrentCapacity() => currCapacity;

    public void UpgradeBoxCapacity(float capacityModif)
    {
        maxCapacity += 3;
        capacityText.text = currCapacity + "/" + maxCapacity;
        UpgradeTruck();
    }

    void TruckFillingVisuals(Transform car)
    {
        Rigidbody rb = car.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        lastCarIdx++;
        car.parent = truckUpgrades;
        car.position = packedCarsPositions[lastCarIdx].position;
        car.rotation = packedCarsPositions[lastCarIdx].rotation;
        car.localScale = packedCarsPositions[lastCarIdx].localScale;
        car.GetComponent<Animation>().Play("Car Truck Scale In Anim");
    }
}
