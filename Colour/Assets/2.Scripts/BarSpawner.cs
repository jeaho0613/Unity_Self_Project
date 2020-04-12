using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    [SerializeField]
    private float delayTime;

    //ObjectPool objectPool;

    private void Start()
    {
      //  objectPool = ObjectPool.Instance;
        StartCoroutine(CreateBar());
    }

    IEnumerator CreateBar()
    {
        while (true)
        {
            int num = Random.Range(0, 4);
            delayTime = Random.Range(1, 5);
            yield return new WaitForSeconds(delayTime);
            //objectPool.SpawnFromPool("BlueBar", transform.position, Quaternion.identity);
        }
    }
}
