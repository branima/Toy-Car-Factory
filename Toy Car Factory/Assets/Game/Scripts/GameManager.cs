using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("UI")]
    public TextMeshProUGUI moneyCount;
    public GameObject upgradeMenu;
    public GameObject upgradeMenuIcon;
    public TextMeshProUGUI trackSpeedLabel;
    public TextMeshProUGUI boxCapacityLabel;

    public Material grayscaleMat;

    public Button trackSpeedButton;
    public Button boxCapacityButton;

    public TextMeshProUGUI nextLevelPriceText;

    [Header("Other Variables")]

    public int trackSpeedUpgradePrice;
    public int boxCapacityUpgradePrice;

    public int maxTrackSpeedUpgradeLvl;
    public int maxBoxCapacityUpgradeLvl;

    public float priceModifier;

    int trackSpeedLevel;
    int boxCapacityLevel;

    [SerializeField]
    int money;

    public float speedModif;
    public float boxCapacityModif;

    public MainTrackLogic trackLogic;
    public SellingBoxLogic boxLogic;

    public int nextLevelPrice;
    
    void Start()
    {
        AddMoney(0);

        trackSpeedLabel.text = "LEVEL " + trackSpeedLevel;
        boxCapacityLabel.text = "LEVEL " + boxCapacityLevel;

        if (nextLevelPrice < 1000)
            nextLevelPriceText.text = "NEXT AREA\n $" + money;
        else if (nextLevelPrice < 1000000)
            nextLevelPriceText.text = "NEXT AREA\n $" + (nextLevelPrice * 1f / 1000).ToString("n1") + "K";
        else
            nextLevelPriceText.text = "NEXT AREA\n $" + (nextLevelPrice * 1f / 1000000).ToString("n1") + "M";
    }

    void Update()
    {
        if (upgradeMenu.activeSelf)
        {
            if (trackSpeedLevel < maxTrackSpeedUpgradeLvl)
            {
                if (money >= trackSpeedUpgradePrice)
                    trackSpeedButton.GetComponent<Image>().material = null;
                else
                    trackSpeedButton.GetComponent<Image>().material = grayscaleMat;
            }
            else
            {
                trackSpeedButton.GetComponent<Image>().material = grayscaleMat;
                trackSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                trackSpeedButton.enabled = false;
            }

            if (boxCapacityLevel < maxBoxCapacityUpgradeLvl)
            {
                if (money >= boxCapacityUpgradePrice)
                    boxCapacityButton.GetComponent<Image>().material = null;
                else
                    boxCapacityButton.GetComponent<Image>().material = grayscaleMat;
            }
            else
            {
                boxCapacityButton.GetComponent<Image>().material = grayscaleMat;
                boxCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                boxCapacityButton.enabled = false;
            }
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;

        if (money < 1000)
            moneyCount.text = "$" + money;
        else if (money < 1000000)
            moneyCount.text = "$" + (money * 1f / 1000).ToString("n1") + "K";
        else
            moneyCount.text = "$" + (money * 1f / 1000000).ToString("n1") + "M";
    }

    public int GetMoney() => money;

    void SetPriceToButton(Button button, int price)
    {
        if (price < 1000)
            button.GetComponentInChildren<TextMeshProUGUI>().text = "$" + price;
        else if (price < 1000000)
            button.GetComponentInChildren<TextMeshProUGUI>().text = "$" + (price * 1f / 1000).ToString("n1") + "K";
        else
            button.GetComponentInChildren<TextMeshProUGUI>().text = "$" + (price * 1f / 1000000).ToString("n1") + "M";
    }

    public void UpgradeTrackSpeed()
    {
        if (money >= trackSpeedUpgradePrice)
        {
            AddMoney(-trackSpeedUpgradePrice);

            trackSpeedUpgradePrice = (int)(trackSpeedUpgradePrice * priceModifier);
            SetPriceToButton(trackSpeedButton, trackSpeedUpgradePrice);

            trackSpeedLabel.text = "LEVEL " + (++trackSpeedLevel);

            if (trackSpeedLevel < maxTrackSpeedUpgradeLvl)
            {
                if (money >= trackSpeedUpgradePrice)
                    trackSpeedButton.GetComponent<Image>().material = null;
                else
                    trackSpeedButton.GetComponent<Image>().material = grayscaleMat;
            }
            else
            {
                trackSpeedButton.GetComponent<Image>().material = grayscaleMat;
                trackSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                trackSpeedButton.enabled = false;
            }

            trackLogic.UpgradeTrackSpeed(speedModif);
        }
    }

    public void UpgradeBoxCapacity()
    {
        if (money >= boxCapacityUpgradePrice)
        {
            AddMoney(-boxCapacityUpgradePrice);

            boxCapacityUpgradePrice = (int)(boxCapacityUpgradePrice * priceModifier);
            SetPriceToButton(boxCapacityButton, boxCapacityUpgradePrice);

            boxCapacityLabel.text = "LEVEL " + (++boxCapacityLevel);

            if (boxCapacityLevel < maxBoxCapacityUpgradeLvl)
            {
                if (money >= boxCapacityUpgradePrice)
                    boxCapacityButton.GetComponent<Image>().material = null;
                else
                    boxCapacityButton.GetComponent<Image>().material = grayscaleMat;
            }
            else
            {
                boxCapacityButton.GetComponent<Image>().material = grayscaleMat;
                boxCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
                boxCapacityButton.enabled = false;
            }

            boxLogic.UpgradeBoxCapacity(boxCapacityModif);
        }
    }

    public void EnableUpgradeMenu()
    {
        upgradeMenu.SetActive(true);
        upgradeMenuIcon.SetActive(false);
    }

    public int GetNextLevelPrice() => nextLevelPrice;

    public void DisableUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        upgradeMenuIcon.SetActive(true);
    }

    public void NextLevel() => LoadLevel((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    void LoadLevel(int levelIndex) => SceneManager.LoadScene(Mathf.Max(0, levelIndex));
}
