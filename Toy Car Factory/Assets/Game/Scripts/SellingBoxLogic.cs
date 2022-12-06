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

    List<Transform> boxContent;

    bool fullCapVisualsOn;
    public Animator textAnimator;
    public Material normalCapTextMat;
    public Material fullCapTextMat;

    public Animator truckAnim;

    public GameObject tutorialArrow;

    void Start()
    {
        capacityText.text = currCapacity + "/" + maxCapacity;
        collectedMoneyText.text = "$0";

        boxContent = new List<Transform>();
        fullCapVisualsOn = false;
        capacityText.fontSharedMaterial = normalCapTextMat;
    }

    void OnTriggerEnter(Collider other)
    {
        ScaleToNothing stn = other.GetComponent<ScaleToNothing>();
        if (stn == null)
        {
            if (currCapacity < maxCapacity)
            {
                if (tutorialArrow != null && !tutorialArrow.activeSelf)
                    tutorialArrow.SetActive(true);

                boxContent.Add(other.transform);
                capacityText.text = ++currCapacity + "/" + maxCapacity;

                collectedMoney += other.GetComponent<BoxAttributes>().GetPrice();
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

                //stn = other.gameObject.AddComponent<ScaleToNothing>();
                //stn.scaleSpeed = 0.25f;
            }
            else
            {
                //stn = other.gameObject.AddComponent<ScaleToNothing>();
                //stn.scaleSpeed = 0.25f;
                ReturnToPool(other.transform);
            }
            other.gameObject.SetActive(false);
        }
    }

    public void ReturnToPool(Transform carBox)
    {
        mainTrackScript.RemoveCarFromList(carBox);
        Transform car = carBox.GetChild(1);
        car.parent = null;
        car.GetComponent<CarAttributes>().GetOriginFactory().EnqueueCar(car);
        carBox.gameObject.SetActive(false);
        ObjectPooler.Instance.Enqueue("Box", carBox.gameObject);
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

        foreach (Transform carBox in boxContent)
        {
            /*
            Transform car = carBox.GetChild(1);
            foreach (Transform wheel in car)
                wheel.gameObject.SetActive(false);
            car.parent = null;
            car.GetComponent<CarAttributes>().GetOriginFactory().EnqueueCar(car);
            carBox.gameObject.SetActive(false);
            ObjectPooler.Instance.Enqueue("Box", carBox.gameObject);
            */
            ReturnToPool(carBox);
        }
        boxContent.Clear();

        currCapacity = 0;
        capacityText.text = currCapacity + "/" + maxCapacity;

        fullCapVisualsOn = false;
        textAnimator.SetBool("full", false);
        capacityText.fontSharedMaterial = normalCapTextMat;
    }

    public int GetCurrentCapacity() => currCapacity;

    public void UpgradeBoxCapacity(float capacityModif)
    {
        maxCapacity = (int)(maxCapacity * capacityModif);
        capacityText.text = currCapacity + "/" + maxCapacity;
    }


}
