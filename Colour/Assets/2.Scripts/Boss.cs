using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Range(0f, 5f)]
    public int endPoint; // 움직임 끝나는 시간
    public float health; // 체력

    private Sequence moveA; // page 1 이동H
   
}
