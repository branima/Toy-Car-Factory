using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryLogic : MonoBehaviour
{

    public List<GameObject> carPrefabs;
    Transform selectedCarPrefab;
    int selectedCarIdx;
    int selectedTyresIdx;
    Transform carInstance;
    [SerializeField]
    Material carPaint;
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
        foreach (Transform tyreSet in selectedCarPrefab)
            tyreSet.gameObject.SetActive(false);
        selectedCarPrefab.GetChild(selectedTyresIdx).gameObject.SetActive(true);
        Transform carInst;
        for (int i = 0; i < numberOfInstances; i++)
        {
            carInst = Instantiate(selectedCarPrefab, Vector3.zero, selectedCarPrefab.rotation, null).transform;
            carInst.GetComponent<CarAttributes>().SetOriginFactory(this);

            Material[] mats = carInst.GetComponent<Renderer>().materials;
            mats[matIdx] = carPaint;
            carInst.GetComponent<Renderer>().materials = mats;
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

    public void SetupCar(int carPrefabIdx, int tyreSetIdx, Material carMat, int matIdx)
    {
        ///CHASSIS + TYRE 
        selectedCarIdx = carPrefabIdx;
        selectedTyresIdx = tyreSetIdx;

        ///MATERIAL
        carPaint = Instantiate(carMat);
        Texture2D texture = new Texture2D(carPaint.mainTexture.width, carPaint.mainTexture.height, TextureFormat.RGBA32, carPaint.mainTexture.mipmapCount, false);
        Graphics.CopyTexture(carPaint.mainTexture, texture);
        carPaint.mainTexture = texture;
        this.matIdx = matIdx;
    }
}
