using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform createPosition;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    public static ObjectPoolManager Instance;

    #region 싱글톤, 오브젝트 풀링
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.prefab.name;
                obj.transform.SetParent(pool.createPosition);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag, objectPool);
        }
    }
    #endregion

    #region 오브젝트 풀에서 생성시 로직
    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"이런 태그는 없습니다. : {tag}");
            return null;
        }

        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPoolCreateSetting pooldeObj = objectToSpawn.GetComponent<IPoolCreateSetting>();

        if(pooldeObj != null)
        {
            pooldeObj.OnObjectSpanw();
        }

        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    #endregion
}
