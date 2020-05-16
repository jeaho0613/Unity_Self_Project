using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Range(0.1f, 1f)] public float spawnDelay;
    string enemy1 = "enemy";

    private void Start()
    {
        StartCoroutine(enemySpawn(enemy1, spawnDelay));
    }

    IEnumerator enemySpawn(string enemyTag, float spawnDelay)
    {
        while(true)
        {
            ObjectPoolManager.Instance.SpawnFromPool(enemyTag, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

