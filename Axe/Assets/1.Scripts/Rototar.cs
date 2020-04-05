using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rototar : MonoBehaviour
{
    public float speed = 100f; // 회전 힘
    private float starRotate = 0f; // y축 회전을 위한 변수

    void Update()
    {
        starRotate += Time.deltaTime; // 시간경과 값을 계속 더해줌

        // 만약 tag가 star변
        if (gameObject.tag == "Star")
        {
            // y축 회전
            // - 짐벌락 현상으로 Quaternion 이용
            transform.rotation = Quaternion.Euler(0, starRotate * speed, 0);
        }
        else
        {
            // z축 회전
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }
}
