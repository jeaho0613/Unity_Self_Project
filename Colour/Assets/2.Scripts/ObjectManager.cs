using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // System.Serializable
    // - 생성한 클래스에 변수들은 모두 public으로 선얺나다.
    // - Awake(), Start()에서 초기화할 필요가 없다. 초기화는 유니티에서 해줌
    // - 변수 선언시 초기화 값을 지정할 수 있다.
    // - Class를 인스펙터 창에 표시를 해준다.
    [System.Serializable]
    public class Pool
    {
        public string tag; // 찾을 태그
        public GameObject prefab; // 생성할 프리팹
        public int size; // 생성 갯수
        public Transform target; // 생성될 오브젝트 위치
    }

    // Pool클래스의 List
    public List<Pool> pools;

    // 오브젝트의 정보가 담기는 Dictionary 배열
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    
    // 싱글톤 변수
    public static ObjectManager Instance;

    #region 싱글톤, 오브젝트 풀링 초기화
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("오브젝트 제거");
            Destroy(gameObject);
        }

        // 생성자로 초기화
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 반복문을 돌린다.
        // - List의 size만큼 반복됨
        foreach (Pool pool in pools)
        {
            // 딕셔너리 배열에 Queue에 담길 obj
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // 반복문을 Pool클래스에 지정해둔 size만큼 반복
            // - 특정 오브젝트의 크기를 지정할 수 있다.
            for (int i = 0; i < pool.size; i++)
            {
                //Debug.Log(Pool.tag);
                GameObject obj = Instantiate(pool.prefab); // obj에 프리팹으로 설정한 게임오브젝트를 생성
                obj.name = pool.prefab.name; // 이름을 프리팹 이름과 통일 (Clone)을 제거하기 위함
                obj.transform.SetParent(pool.target); // 생성될 위치값
                obj.SetActive(false); // 비활성화
                objectPool.Enqueue(obj); // queue배열에 담는다.
            }

            // 딕셔너리 배열에 저장
            // - Pool.tag의 이름으로 objectPool에 담긴 게임오브젝트를 관리할 수 있다.
            // - ex) BulletR(Clone)들은 Red태그로 묶여있어 Red를 검색하여 오브젝트를 관리
            PoolDictionary.Add(pool.tag, objectPool);
            //Debug.Log($"Pool.tag : {pool.tag}, obejctPool : {objectPool.GetType()}"); // 생성된 정보 확인 로그
            //Debug.Log(PoolDictionary.Count);
        }
    }
    #endregion

    #region 오브젝트 풀에서 생성할 때 로직
    public GameObject SpawnFromPool(string tag, Vector3 postion, Quaternion rotation)
    {

        // tag를 잘못 입력할 경우 처리
        // - ContainsKey : 키값을 입력하여 자료를 검색함.
        // - 그 키 값이 없다면 오류로그를 출력하고 리턴
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"이런 태그는 없습니다. : {tag}");
            return null;
        }

        // 딕셔너리에 tag로 검색하여(Queue 배열) 1개씩 빼줌
        // Queue : FIFO (선입선출 구조) 먼저들어온 데이터를 처리할 경우 용이함
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        // objectToSpawn은 순차적으로 queue배열의 오브젝트를 할당받는다.
        objectToSpawn.SetActive(true); // 활성화 
        objectToSpawn.transform.position = postion; // 위치
        objectToSpawn.transform.rotation = rotation; // 회전

        // 반복사용을 위한 부분
        // - objectToSpawn에 상속된 interface를 가져온다.
        // - 현재의 경우 Bullt.cs에 IPooledObejct가 상속된 상황
        IPooledObject PooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (PooledObj != null)
        {
            // Bullet.cs의 OnObjectSpawn을 실행시킨다.
            PooledObj.OnObjectSpanw();
        }

        // 다쓴 오브젝트는 다시 딕셔너리 배열에 저장.
        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    #endregion
}