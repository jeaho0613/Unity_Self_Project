using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rototar : MonoBehaviour
{
    public float speed = 100f;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
