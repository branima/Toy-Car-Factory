using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryLogic : MonoBehaviour
{

    public List<GameObject> carPrefabs;
    Transform selectedCarPrefab;
    int selectedCarIdx;
    int selectedTyresIdx;
    int selectedSpoilerIdx;
    Transform carInstance;
    List<Material> carPaint;
    int matIdx;

    public int numberOfInstances = 200;
    Queue<Transform> carPool;

    public Transform spawnPosition;

    public float spawnSpeed;
    float lastSpawnTime;

    MiniTrackLogic miniTrackScript;

    public Transform mainGameplayScene;

    void Start()
    {
        lastSpawnTime = -1000f;
        miniTrackScript = GetComponentInChildren<MiniTrackLogic>();
        carPool = new Queue<Transform>();
        selectedCarPrefab = carPrefabs[selectedCarIdx].transform;
        Debug.Log(selectedCarPrefab.GetChild(selectedCarPrefab.childCount - 2) + ", " + selectedCarPrefab.GetChild(selectedCarPrefab.childCount - 1));
        selectedCarPrefab.GetChild(selectedCarPrefab.childCount - 2).GetChild(selectedSpoilerIdx).gameObject.SetActive(true);
        selectedCarPrefab.GetChild(selectedCarPrefab.childCount - 1).GetChild(selectedTyresIdx).gameObject.SetActive(true);
        Transform carInst;
        for (int i = 0; i < numberOfInstances; i++)
        {
            carInst = Instantiate(selectedCarPrefab, Vector3.zero, selectedCarPrefab.rotation, null).transform;
            carInst.GetComponent<CarAttributes>().SetOriginFactory(this);

            for (int j = 0; j < 3; j++)
            {
                Renderer rend = carInst.GetChild(j).GetComponent<Renderer>();
                Material[] mats = rend.materials;
                for (int k = 0; k < 2; k++)
                    mats[k] = carPaint[2 * j + k];
                rend.materials = mats;
            }

            Transform spoiler = carInst.GetChild(carInst.childCount - 2).GetChild(selectedSpoilerIdx);
            spoiler.GetComponent<MeshRenderer>().sharedMaterial = carPaint[0];
            foreach (Transform item in spoiler)
                item.GetComponent<MeshRenderer>().sharedMaterial = carPaint[2];

            carPool.Enqueue(carInst);
        }
    }

    void Update()
    {
        if (Time.time - lastSpawnTime > spawnSpeed)
        {
            carInstance = carPool.Dequeue();
            carInstance.position = spawnPosition.position;
            carInstance.rotation = spawnPosition.rotation;
            carInstance.localScale = selectedCarPrefab.localScale;
            carInstance.gameObject.SetActive(true);
            miniTrackScript.AddCarToTrack(carInstance.transform);
            lastSpawnTime = Time.time;
        }
    }

    public void EnqueueCar(Transform car)
    {
        car.gameObject.SetActive(false);
        car.GetComponent<Collider>().enabled = true;
        carPool.Enqueue(car);
    }

    //public void SetCarMat(Material carMat) => /// posalji which car, which wheels

    public void SetupCar(int carPrefabIdx, int tyreSetIdx, int spoilerIdx, List<MeshRenderer> meshRenderers)
    {
        ///CHASSIS + TYRE 
        selectedCarIdx = carPrefabIdx;
        selectedTyresIdx = tyreSetIdx;
        selectedSpoilerIdx = spoilerIdx;

        carPaint = new List<Material>();
        foreach (MeshRenderer item in meshRenderers)
            foreach (Material mat in item.materials)
                carPaint.Add(Instantiate(mat));
    }
}
