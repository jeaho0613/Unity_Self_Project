using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleControll : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.DOMove(new Vector3(0, -5.5f, 0), 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(other.gameObject);
        }    
    }

}
