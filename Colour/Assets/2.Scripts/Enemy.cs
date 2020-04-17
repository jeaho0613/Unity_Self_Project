using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IPooledObject
{
    [Range(1f, 10f)]
    public float endSpeed; // 도착지점에 도달되는 시간
    [Range(0f, 5f)]
    public float shootingDelay; // 총알 간격
    public float health; // 체력
    public int power; // 공격력
    public Sprite[] sprites; // 피격 효과시 교체될 이미지

    private float savehealth; // 재 생성될때 저장 체력값
    private SpriteRenderer spriteRenderer; // enemy 기체 이미지렌더러
    private string[] enemyType = { "EBS", "EBM", "EBL" }; // 풀링에 대입될 배열
    private float startTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 초기화
        savehealth = health; // 체력값을 save에 저장
        
    }

    private void Update()
    {
        startTime += Time.deltaTime;
        if (startTime >= shootingDelay)
        {
            Shooting();
        }
    }

    public void OnObjectSpanw()
    {
        Shooting();
        health = savehealth;
    }

    // 충돌 로직
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 벽과 충돌했을 경우
        if (other.tag == "Wall")
        {
            gameObject.SetActive(false);
        }

        // playerBullet과 충돌했을 경우
        if (other.tag == "PBullet")
        {
            //Debug.Log("총알과 충돌");
            other.gameObject.SetActive(false); // 총알은 비활성화

            spriteRenderer.sprite = sprites[1]; // 이미지 변경
            health -= other.GetComponent<Bullt>().power; // 총알의 데미지 만큼 체력 제거
            Invoke("Attacked", 0.1f); // 이미지 복원

            // 만약 체력이 0보다 작아질 경우 비활성화
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // 피격시 스프라이트 복구
    private void Attacked()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void Shooting()
    {
        ObjectManager.Instance.SpawnFromPool("EBS", transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        startTime = 0;
    }
}
