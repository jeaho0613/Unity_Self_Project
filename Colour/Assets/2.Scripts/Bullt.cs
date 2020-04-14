using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullt : MonoBehaviour, IPooledObject
{
    public string bullteType; // 총알 타입
    [Range(0f, 10f)]
    public float endTime; // 끝나는 시간
    [Range(0f, 20f)]
    public float power; // 총알 힘

    public void OnObjectSpanw()
    {
        transform.DOLocalMoveY(5.5f, endTime).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false); // 시간지나면 비활성화
            });
    }

}
