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
    public Material bodyMat;
    public Material haubaMat;
    public Material roofMat;
    public Material bumperMat;

    [Header("Spoiler")]
    public GameObject spoilerPanel;
    Transform spoilerGroup;
    int activeSpoilerIdx;

    [Header("Hood")]
    public GameObject hoodPanel;
    Transform hoodGroup;
    int activeHoodIdx;

    [Header("Bumper")]
    public GameObject bumperPanel;
    Transform bumperGroup;
    int activeBumperIdx;

    [Header("Stickering")]
    public GameObject stickeringPanel;
    List<Material> matsContainer;

    [Header("Tyre Selection")]
    public GameObject tyreSelectionPanel;
    Transform tyreSetsParent;
    int activeTyreIdx;

    public void EnableChasisSelection()
    {
        bodyMat.SetColor("_oldColor", Color.white);
        bodyMat.SetColor("_newColor", Color.white);

        haubaMat.SetColor("_oldColor", Color.white);
        haubaMat.SetColor("_newColor", Color.white);

        bumperMat.SetColor("_oldColor", Color.white);
        bumperMat.SetColor("_newColor", Color.white);

        roofMat.SetColor("_oldColor", Color.white);
        roofMat.SetColor("_newColor", Color.white);

        foreach (GameObject chassis in chasisList)
            chassis.SetActive(false);

        chasisSelectionPanel.SetActive(true);
        activeCarIdx = 0;
        chasisList[activeCarIdx].gameObject.SetActive(true);

        angleIdx = 0;
        tyreSelectionPanel.SetActive(false);
        CameraSwitch.Instance.SetOgPosition();

        foreach (GameObject chassis in chasisList)
        {
            foreach (Transform carPart in chassis.transform)
            {
                if (carPart.GetComponent<Renderer>() == null)
                {
                    foreach (Transform child in carPart)
                        child.gameObject.SetActive(false);
                    carPart.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        activeCarIdx = 0;
        activeTyreIdx = 0;
        activeSpoilerIdx = 0;
        activeHoodIdx = 0;
        activeBumperIdx = 0;
    }

    public void ChasisSwitch(int newChasisIdx)
    {
        chasisList[activeCarIdx].gameObject.SetActive(false);
        activeCarIdx = newChasisIdx;
        chasisList[activeCarIdx].SetActive(true);
    }

    public void ConfirmChasis()
    {
        activeCarMeshRenderers = new List<MeshRenderer>();
        activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(0).GetComponent<MeshRenderer>());

        spoilerGroup = chasisList[activeCarIdx].transform.GetChild(4);
        hoodGroup = chasisList[activeCarIdx].transform.GetChild(1);
        bumperGroup = chasisList[activeCarIdx].transform.GetChild(2);
        ChangeCamAngle();
    }

    public void EnablePaintingPanel()
    {
        chasisSelectionPanel.SetActive(false);
        hoodPanel.SetActive(false);
        bumperPanel.SetActive(false);
        paintingPanel.SetActive(true);

        if (angleIdx == 2)
            activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(1).GetChild(activeHoodIdx).GetComponent<MeshRenderer>());
        else if (angleIdx == 3)
            activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(2).GetChild(activeBumperIdx).GetComponent<MeshRenderer>());
        else if (angleIdx == 4)
            activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(3).GetComponent<MeshRenderer>());
        else if (angleIdx == 5)
            activeCarMeshRenderers.Add(chasisList[activeCarIdx].transform.GetChild(4).GetChild(activeSpoilerIdx).GetComponent<MeshRenderer>());
    }

    int angleIdx = 0;

    public void ChangeCamAngle()
    {
        angleIdx++;
        CameraSwitch.Instance.ChangeCamera();
        if (angleIdx == 1) /// BODY PAINTING
        {
            EnablePaintingPanel();
        }
        if (angleIdx == 2) /// HOOD
        {
            EnableHoodSelection();
        }
        else if (angleIdx == 3) /// BUMPER
        {
            EnableBumperSelection();
        }
        else if (angleIdx == 4) /// ROOF
        {
            EnablePaintingPanel();
        }
        else if (angleIdx == 5) /// SPOILER 
        {
            activeSpoilerIdx = 0;
            paintingPanel.SetActive(false);
            spoilerPanel.SetActive(true);

            bodyMat.SetColor("_oldColor", bodyMat.GetColor("_newColor"));
            haubaMat.SetColor("_oldColor", haubaMat.GetColor("_newColor"));
            roofMat.SetColor("_oldColor", roofMat.GetColor("_newColor"));
        }
        else if (angleIdx == 6) /// STICKERING
        {
            matsContainer = chasisList[activeCarIdx].GetComponent<StickersDecalsMatsContainer>().GetStickersList();
            spoilerPanel.SetActive(false);
            stickeringPanel.SetActive(true);
        }
        else if (angleIdx == 7) /// TYRES
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
            bodyMat.SetColor("_oldColor", bodyMat.GetColor("_newColor"));
            bodyMat.SetColor("_newColor", mat.color);
            activeCarMeshRenderers[0].GetComponent<Animation>().Play("Body Fill Anim");
        }
        else if (angleIdx == 2)
        { /// Hauba
            haubaMat.SetColor("_oldColor", haubaMat.GetColor("_newColor"));
            haubaMat.SetColor("_newColor", mat.color);
            //activeCarMeshRenderers[1].GetComponent<Animation>().Play("Hauba Fill Anim");
            chasisList[activeCarIdx].transform.GetChild(1).GetChild(activeHoodIdx).GetComponent<Animation>().Play("Hauba Fill Anim");
        }
        else if (angleIdx == 3)
        { /// Bumper
            bumperMat.SetColor("_oldColor", bumperMat.GetColor("_newColor"));
            bumperMat.SetColor("_newColor", mat.color);
            //activeCarMeshRenderers[2].GetComponent<Animation>().Play("Roof Fill Anim");
            chasisList[activeCarIdx].transform.GetChild(2).GetChild(activeBumperIdx).GetComponent<Animation>().Play("Bumper Fill Anim");
        }
        else if (angleIdx == 4)
        { /// Roof
            roofMat.SetColor("_oldColor", roofMat.GetColor("_newColor"));
            roofMat.SetColor("_newColor", mat.color);
            activeCarMeshRenderers[3].GetComponent<Animation>().Play("Roof Fill Anim");
        }
    }

    public void EnableHoodSelection()
    {
        paintingPanel.SetActive(false);
        hoodPanel.SetActive(true);
    }

    public void EnableBumperSelection()
    {
        paintingPanel.SetActive(false);
        bumperPanel.SetActive(true);
    }

    public void SelectHood(int hoodIdx)
    {
        hoodGroup.GetChild(activeHoodIdx).gameObject.SetActive(false);
        activeHoodIdx = hoodIdx;
        hoodGroup.GetChild(activeHoodIdx).gameObject.SetActive(true);
    }

    public void SelectBumper(int bumperIdx)
    {
        bumperGroup.GetChild(activeBumperIdx).gameObject.SetActive(false);
        activeBumperIdx = bumperIdx;
        bumperGroup.GetChild(activeBumperIdx).gameObject.SetActive(true);
    }

    public void SelectSpoiler(int spoilerIdx)
    {
        spoilerGroup.GetComponent<Animation>().Play("Spoiler Anim");
        spoilerGroup.GetChild(activeSpoilerIdx).gameObject.SetActive(false);
        activeSpoilerIdx = spoilerIdx;
        spoilerGroup.GetChild(activeSpoilerIdx).gameObject.SetActive(true);
    }

    public void SelectStickers(int stickerIdx)
    {
        Material mat = matsContainer[stickerIdx];
        foreach (MeshRenderer meshRenderer in activeCarMeshRenderers)
        {
            Material[] mats = meshRenderer.sharedMaterials;
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
        tyreSetsParent.GetChild(activeTyreIdx).GetComponent<Animation>().Play("Tyre Set " + (activeTyreIdx + 1) + " Anim");
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

        //int matIdx = chasisList[activeCarIdx].GetComponent<P3dPaintableTexture>().Slot.Index;
        purchaseLogicScript.GetLatestFactory().SetupCar(activeCarIdx, activeTyreIdx, activeSpoilerIdx, activeHoodIdx, activeBumperIdx, activeCarMeshRenderers);
    }
}
