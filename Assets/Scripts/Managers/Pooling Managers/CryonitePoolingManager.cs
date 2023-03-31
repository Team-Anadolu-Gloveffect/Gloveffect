using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryonitePoolingManager : MonoBehaviour
{
    #region Singleton

    public static CryonitePoolingManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private GameObject poolParent;
    [SerializeField] private Queue<GameObject> PoolableObjectList = new Queue<GameObject>();
    //[SerializeField] private GameObject poolPrefab;
    //[SerializeField] private int poolAmount = 100;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public void Start()
    {
        Setup();
    }

    private void Setup()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            PoolableObjectList = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var go = Instantiate(pool.prefab, poolParent.transform, true);
                go.SetActive(false);

                PoolableObjectList.Enqueue(go);
            }
            poolDictionary.Add(pool.tag, PoolableObjectList);
        }      
    }

    public void EnqueueBullet(GameObject poolObject)
    {
        poolObject.transform.parent = poolParent.transform;
        poolObject.transform.localPosition = Vector3.zero;
        poolObject.transform.localEulerAngles = Vector3.zero;

        poolObject.gameObject.SetActive(false);

        //PoolableObjectList.Enqueue(poolObject);
        poolDictionary[tag].Enqueue(poolObject);
    }

    public GameObject DequeuePoolableGameObject()
    {
        var deQueuedPoolObject = poolDictionary[tag].Dequeue();
        if (deQueuedPoolObject.activeSelf) DequeuePoolableGameObject();
        deQueuedPoolObject.SetActive(true);
        return deQueuedPoolObject;
    }
}
