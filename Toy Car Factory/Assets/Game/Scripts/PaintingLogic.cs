using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;

public class PaintingLogic : MonoBehaviour
{
    [Header("Main Scene Switch Components")]
    public GameObject mainGameplayScene;
    public GameObject gameplayCanvas;

    public GameObject diyScene;
    public GameObject diyCanvas;

    public PurchaseLogic purchaseLogicScript;

    public List<GameObject> purchaseSlots;

    public GameObject swipeToMoveTutorialPanel;

    [Header("Chasis Selection")]

    public List<GameObject> chasisList;
    public GameObject chasisSelectionPanel;
    int activeCarIdx;

    [Header("Painting")]

    public GameObject paintingPanel;
    public GameObject colorView;
    public CameraRotation rotationScript;

    public P3dPaintSphere brush;

    [Header("Tyre Selection")]

    public GameObject tyreSelectionPanel;
    int activeTyreIdx;

    public void SelectColor(Material colorMat) => brush.Color = colorMat.color;

    public void EnableChasisSelection()
    {
        foreach (GameObject chassis in chasisList)
            chassis.SetActive(false);

        chasisSelectionPanel.SetActive(true);
        activeCarIdx = 0;
        chasisList[activeCarIdx].gameObject.SetActive(true);
    }

    public void ChasisSwitch(int newChasisIdx)
    {
        chasisList[activeCarIdx].gameObject.SetActive(false);
        chasisList[newChasisIdx].transform.rotation = chasisList[activeCarIdx].transform.rotation;
        activeCarIdx = newChasisIdx;
        chasisList[activeCarIdx].SetActive(true);
    }

    public void EnablePaintingPanel()
    {
        chasisSelectionPanel.SetActive(false);

        paintingPanel.SetActive(true);
        Toggle[] toggles = paintingPanel.GetComponentsInChildren<Toggle>();
        foreach (Toggle item in toggles)
            item.isOn = false;
        paintingPanel.transform.GetChild(paintingPanel.transform.childCount - 1).gameObject.SetActive(false);
    }

    public void EnableColorSelection()
    {
        colorView.SetActive(true);
        rotationScript.enabled = false;
        brush.gameObject.SetActive(true);
    }

    public void EnableTyreSelection()
    {
        foreach (GameObject item in chasisList)
            item.transform.GetChild(0).GetComponent<Animator>().enabled = true;

        paintingPanel.SetActive(false);
        rotationScript.enabled = false;
        brush.gameObject.SetActive(false);

        tyreSelectionPanel.SetActive(true);
        activeTyreIdx = 0;
    }

    public void TyreSwitch(int newTyreIdx)
    {
        chasisList[activeCarIdx].transform.GetChild(activeTyreIdx).gameObject.SetActive(false);
        activeTyreIdx = newTyreIdx;
        chasisList[activeCarIdx].transform.GetChild(activeTyreIdx).gameObject.SetActive(true);
    }

    public void ToggleRotation(Toggle toggle)
    {
        if (toggle.isOn)
            rotationScript.enabled = true;
        else
            rotationScript.enabled = false;

        colorView.SetActive(false);
        brush.gameObject.SetActive(false);
    }

    public void Confirm()
    {
        tyreSelectionPanel.SetActive(false);

        if (!swipeToMoveTutorialPanel.activeSelf)
            swipeToMoveTutorialPanel.SetActive(true);

        if (purchaseSlots.Count > 0)
        {
            foreach (GameObject slot in purchaseSlots)
                slot.SetActive(true);
            purchaseSlots.Clear();
        }

        diyScene.SetActive(false);
        diyCanvas.SetActive(false);
        mainGameplayScene.SetActive(true);
        gameplayCanvas.SetActive(true);

        int matIdx = chasisList[activeCarIdx].GetComponent<P3dPaintableTexture>().Slot.Index;
        purchaseLogicScript.GetLatestFactory().SetupCar(activeCarIdx, activeTyreIdx, chasisList[activeCarIdx].GetComponent<Renderer>().materials[matIdx], matIdx);
    }
}
