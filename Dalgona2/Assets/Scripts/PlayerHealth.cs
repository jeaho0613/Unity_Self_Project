using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float _health;
    public float health 
    {
        get
        {
            return _health;
        }
        set
        {
            if(value <= 0)
            {
                _health = 0;
                Die();
                return;
            }
            else
            {
                _health = value;
            }
        }
    }

    private void Awake()
    {
        _health = 100f;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
