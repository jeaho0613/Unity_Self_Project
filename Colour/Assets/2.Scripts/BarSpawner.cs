using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    [SerializeField]
    private float delayTime;
    
    public GameObject[] barPreFabs;

    private void Start()
    {
        StartCoroutine(CreateBar());
    }

    IEnumerator CreateBar()
    {
        while (true)
        {
            int num = Random.Range(0, 4);
            delayTime = Random.Range(1, 5);
            yield return new WaitForSeconds(delayTime);
            Instantiate(barPreFabs[num], transform.position, transform.rotation);
        }
    }
}
