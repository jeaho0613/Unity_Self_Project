using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullt : MonoBehaviour
{
    public string bulltType;
    [Range(1f, 10f)]
    public float endTime; // 끝나는 시간
    [Range(1f, 10f)]
    public float power; // 총알 힘

    void Start()
    {
        transform.DOLocalMoveY(5.5f, endTime).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

}
