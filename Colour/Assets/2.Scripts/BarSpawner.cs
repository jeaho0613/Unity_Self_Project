using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    private float delayTime;
    private string[] colos = { "BarR", "BarG", "BarB"};
   
    private void Start()
    {
        StartCoroutine(CreateBar());
    }

    IEnumerator CreateBar()
    {
        while (true)
        {
            int num = Random.Range(0, 3);
            delayTime = Random.Range(1, 5);
            yield return new WaitForSeconds(delayTime);
            ObjectManager.Instance.SpawnFromPool(colos[num], transform.position, Quaternion.identity);
        }
    }
}
