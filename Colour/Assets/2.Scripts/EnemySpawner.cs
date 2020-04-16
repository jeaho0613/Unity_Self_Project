using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] points;
    public int index;
    public float nextSpawnTime;

    private List<GameManager.Spawn> enemySpawnList;

    private void Update()
    {
        nextSpawnTime += Time.deltaTime;
    }

    private void Awake()
    {
        index = 0;
        enemySpawnList = GameManager.Instance.spawnList;
    }

    private void Start()
    {
        StartCoroutine(enemySpawn());
    }

    IEnumerator enemySpawn()
    {
        yield return new WaitForSeconds(enemySpawnList[index].delay);
        ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type, points[enemySpawnList[index].point].position, transform.rotation);
        index++;
    }
}
