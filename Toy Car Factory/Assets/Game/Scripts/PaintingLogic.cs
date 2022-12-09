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
    List<MeshRenderer> activeCarMeshRenderers;

    [Header("Painting")]
    public GameObject paintingPanel;

    [Header("Spoiler")]
    public GameObject spoilerPanel;
    Transform spoilerGroup;

    [Header("Stickering")]
    public GameObject stickeringPanel;
    List<Material> matsContainer;

    [Header("Tyre Selection")]
    public GameObject tyreSelectionPanel;
    Transform tyreSetsParent;
    int activeTyreIdx;

    public void EnableChasisSelection()
    {
        ///CameraSwitch.Instance.SetOgPosition();

        foreach (GameObject chassis in chasisList)
        {
            /*  /// NEW IMPLEMENTATION PURPOSES
            chassis.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            foreach (Transform tyreSet in chassis.transform)
                tyreSet.gameObject.SetActive(false);
            chassis.transform.GetChild(0).gameObject.SetActive(true);
            */
            chassis.SetActive(false);
        }

        chasisSelectionPanel.SetActive(true);
        activeCarIdx = 0;
        chasisList[activeCarIdx].gameObject.SetActive(true);
    }

    public void ChasisSwitch(int newChasisIdx)
    {
        chasisList[activeCarIdx].gameObject.SetActive(false);
        //chasisList[newChasisIdx].transform.rotation = chasisList[activeCarIdx].transform.rotation;
        activeCarIdx = newChasisIdx;
        chasisList[activeCarIdx].SetActive(true);
    }

    public void EnablePaintingPanel()
    {
        chasisSelectionPanel.SetActive(false);
        paintingPanel.SetActive(true);
        activeCarMeshRenderers = new List<MeshRenderer>();
        activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(0).GetComponent<MeshRenderer>());
        activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(1).GetComponent<MeshRenderer>());
        activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(2).GetComponent<MeshRenderer>());
        spoilerGroup = chasisList[activeCarIdx].transform.GetChild(3);
        ChangeCamAngle();
    }

    int angleIdx = 0;

    public void ChangeCamAngle()
    {
        angleIdx++;
        CameraSwitch.Instance.ChangeCamera();
        if (angleIdx == 4) /// SPOILER 
        {
            paintingPanel.SetActive(false);
            spoilerPanel.SetActive(true);

            for (int i = 1; i < spoilerGroup.childCount; i++)
            {
                spoilerGroup.GetChild(i).GetComponent<MeshRenderer>().material = activeCarMeshRenderers[1].material;
                foreach (Transform winglets in spoilerGroup.GetChild(i))
                    winglets.GetComponent<MeshRenderer>().material = activeCarMeshRenderers[0].material;
            }
        }
        else if (angleIdx == 5) /// STICKERING
        {
            matsContainer = chasisList[activeCarIdx].GetComponent<StickersDecalsMatsContainer>().GetStickersList();
            spoilerPanel.SetActive(false);
            stickeringPanel.SetActive(true);
        }
        else if (angleIdx == 6) /// TYRES
        {
            stickeringPanel.SetActive(false);
            EnableTyreSelection();
        }
    }

    public void SetColor(Material mat) => activeCarMeshRenderers[angleIdx - 1].material = mat;

    public void SelectSpoiler(int spoilerIdx)
    {
        foreach (Transform item in spoilerGroup)
            item.gameObject.SetActive(false);

        spoilerGroup.GetChild(spoilerIdx).gameObject.SetActive(true);
    }

    public void SelectStickers(int stickerIdx)
    {
        Material mat = matsContainer[stickerIdx];
        foreach (MeshRenderer meshRenderer in activeCarMeshRenderers)
        {
            Material[] mats = meshRenderer.materials;
            mats[mats.Length - 1] = mat;
            meshRenderer.materials = mats;
        }
    }

    public void EnableTyreSelection()
    {
        tyreSelectionPanel.SetActive(true);
        tyreSetsParent = chasisList[activeCarIdx].transform.GetChild(chasisList[activeCarIdx].transform.childCount - 1);
        activeTyreIdx = 0;
    }

    public void TyreSwitch(int newTyreIdx)
    {
        tyreSetsParent.GetChild(activeTyreIdx).gameObject.SetActive(false);
        activeTyreIdx = newTyreIdx;
        tyreSetsParent.GetChild(activeTyreIdx).gameObject.SetActive(true);
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
