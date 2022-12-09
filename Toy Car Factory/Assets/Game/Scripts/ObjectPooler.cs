using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public GameObject mainGameplayScene;

    public static ObjectPooler Instance;
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = mainGameplayScene.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (poolDictionary[tag].Count > 0)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
        return null;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary[tag].Count > 0)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn;
        }
        return null;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, bool oldRotation)
    {
        if (poolDictionary[tag].Count > 0)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;

            return objectToSpawn;
        }
        return null;
    }

    //public void Enqueue(string tag, GameObject instance) => poolDictionary[tag].Enqueue(instance);
    public void Enqueue(string tag, GameObject instance)
    {
        foreach (Pool pool in pools)
        {
            if (pool.tag == tag)
            {
                instance.transform.position = pool.prefab.transform.position;
                instance.transform.localScale = pool.prefab.transform.localScale;
                instance.transform.rotation = pool.prefab.transform.rotation;
                instance.GetComponent<Collider>().material = null;
                poolDictionary[tag].Enqueue(instance);
            }
        }

    }
}