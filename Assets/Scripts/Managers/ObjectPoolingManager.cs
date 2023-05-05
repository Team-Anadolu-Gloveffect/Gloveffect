using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    #region Singleton
    public static ObjectPoolingManager Instance;
    private void Awake()
    {
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

    [SerializeField] private List<Pool> pools;
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] private GameObject poolParent;

    void Start()
    {
        PoolingSetup();
    }
    public void PoolingSetup()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var spell = Instantiate(pool.prefab, poolParent.transform, true);
                spell.SetActive(false);
                objectPool.Enqueue(spell);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        //objectToReturn.GetComponent<Spell>().direction = Vector3.zero;
        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
    public bool IsPoolAllActive(string tag)
    {
        Queue<GameObject> pool = poolDictionary[tag];
        foreach (GameObject obj in pool)
        {
            if (obj == null || !obj.activeInHierarchy)
            {
                Debug.Log("asdjasd");
                return false;
            }
        }
        return true;
    }
}
