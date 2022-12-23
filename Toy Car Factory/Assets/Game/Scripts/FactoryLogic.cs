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
    int selectedHoodIdx;
    int selectedBumperIdx;
    Transform carInstance;
    [SerializeField]
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
        Transform carInst;
        List<Renderer> renderers = new List<Renderer>();
        for (int i = 0; i < numberOfInstances; i++)
        {
            carInst = Instantiate(selectedCarPrefab, Vector3.zero, selectedCarPrefab.rotation, null).transform;
            carInst.GetComponent<CarAttributes>().SetOriginFactory(this, miniTrackScript);

            carInst.GetChild(1).GetChild(selectedHoodIdx).gameObject.SetActive(true);
            carInst.GetChild(2).GetChild(selectedBumperIdx).gameObject.SetActive(true);
            carInst.GetChild(4).GetChild(selectedSpoilerIdx).gameObject.SetActive(true);
            carInst.GetChild(5).GetChild(selectedTyresIdx).gameObject.SetActive(true);

            renderers.Add(carInst.GetChild(0).GetComponent<Renderer>());
            renderers.Add(carInst.GetChild(1).GetChild(selectedHoodIdx).GetComponent<Renderer>());
            renderers.Add(carInst.GetChild(2).GetChild(selectedBumperIdx).GetComponent<Renderer>());
            renderers.Add(carInst.GetChild(3).GetComponent<Renderer>());

            for (int j = 0; j < 4; j++)
            {
                Material[] mats = renderers[j].sharedMaterials;
                for (int k = 0; k < 2; k++)
                    mats[k] = carPaint[2 * j + k];
                renderers[j].sharedMaterials = mats;
            }
            renderers.Clear();

            if (selectedSpoilerIdx != 0)
            {
                Transform spoiler = carInst.GetChild(4).GetChild(selectedSpoilerIdx);
                spoiler.GetComponent<MeshRenderer>().sharedMaterial = carPaint[0];
                foreach (Transform item in spoiler)
                    item.GetComponent<MeshRenderer>().sharedMaterial = carPaint[2];
            }
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
        Rigidbody rb = car.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        carPool.Enqueue(car);
    }

    public void SetupCar(int carPrefabIdx, int tyreSetIdx, int spoilerIdx, int hoodIdx, int bumperIdx, List<MeshRenderer> meshRenderers)
    {
        ///CHASSIS + TYRE 
        selectedCarIdx = carPrefabIdx;
        selectedTyresIdx = tyreSetIdx;
        selectedSpoilerIdx = spoilerIdx;
        selectedHoodIdx = hoodIdx;
        selectedBumperIdx = bumperIdx;

        carPaint = new List<Material>();
        foreach (MeshRenderer item in meshRenderers)
            foreach (Material mat in item.sharedMaterials)
                carPaint.Add(Instantiate(mat));
    }
}
