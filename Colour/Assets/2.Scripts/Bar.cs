using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bar : MonoBehaviour
{
    public string currentType; // Bar의 Type
    void Start()
    {
        transform.DOMove(new Vector3(0, -5.5f, 0), 5f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentColor = other.GetComponent<PlayerControll>().currentColor;
        
        if(!(currentColor == currentType))
        {
            Destroy(other.gameObject);
        }
    }
}
