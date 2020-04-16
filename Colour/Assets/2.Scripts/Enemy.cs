using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public string type;
    public int health;
    public int power;

    void Start()
    {
        transform.DOLocalMoveY(5.5f, 2);
    }
}
