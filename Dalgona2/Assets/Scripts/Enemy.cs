using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(10f,50f)]public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<PlayerHealth>();
        if (target != null)
        {
            target.health -= damage;
        }
    }
}
