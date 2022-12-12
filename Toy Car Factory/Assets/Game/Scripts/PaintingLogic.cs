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
    public Renderer bodyRenderer;
    public Renderer haubaRenderer;
    public Renderer roofRenderer;

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
        foreach (GameObject chassis in chasisList)
            chassis.SetActive(false);

        chasisSelectionPanel.SetActive(true);
        activeCarIdx = 0;
        chasisList[activeCarIdx].gameObject.SetActive(true);
    }

    public void ChasisSwitch(int newChasisIdx)
    {
        chasisList[activeCarIdx].gameObject.SetActive(false);
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

    //public void SetColor(Material mat) => activeCarMeshRenderers[angleIdx - 1].material = mat;
    public void SetColor(Material mat)
    {
        if (angleIdx == 1)
        {  /// Body
            Material bodyMat = bodyRenderer.sharedMaterial;
            bodyMat.SetColor("_oldColor", bodyMat.GetColor("_newColor"));
            bodyMat.SetColor("_newColor", mat.color);
            bodyRenderer.GetComponent<Animation>().Play("Body Fill Anim");
        }
        else if (angleIdx == 2)
        { /// Hauba
            Debug.Log("Ben");
            Material haubaMat = haubaRenderer.sharedMaterial;
            haubaMat.SetColor("_oldColor", haubaMat.GetColor("_newColor"));
            haubaMat.SetColor("_newColor", mat.color);
            haubaRenderer.GetComponent<Animation>().Play("Hauba Fill Anim");
        }
        else if (angleIdx == 3)
        { /// Roof
            Material roofMat = roofRenderer.sharedMaterial;
            roofMat.SetColor("_oldColor", roofMat.GetColor("_newColor"));
            roofMat.SetColor("_newColor", mat.color);
            roofRenderer.GetComponent<Animation>().Play("Roof Fill Anim");

        }
    }

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
