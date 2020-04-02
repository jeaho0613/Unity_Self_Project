using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rototar : MonoBehaviour
{
    public float speed = 100f;
    private float starRotate = 0f;
    
    void Update()
    {
        starRotate += Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));

        if (gameObject.tag == "Star")
        {
            transform.rotation = Quaternion.Euler(0,starRotate * speed,0);
        }
    }
}
