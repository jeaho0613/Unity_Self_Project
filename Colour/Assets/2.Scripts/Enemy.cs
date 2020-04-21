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
    public float enemyPoint; // SP 포인트
    public Sprite[] sprites; // 피격 효과시 교체될 이미지
    public string enemyType; // 현재 기체 타입

    private float savehealth; // 재 생성될때 저장 체력값
    private SpriteRenderer spriteRenderer; // enemy 기체 이미지렌더러
    private float startTime; // 총알 발사 주기

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 초기화
        savehealth = health; // 체력값을 save에 저장
    }

    private void Update()
    {
        startTime += Time.deltaTime; // 경과 시간

        // 경과 시간 >= 적 총알 생성 주기
        if (startTime >= shootingDelay)
        {
            Shooting(); // 적 총알 발사
        }
    }

    // 생성될 때 실행될 로직
    public void OnObjectSpanw()
    {
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

            // 만약 체력이 0보다 작아질 경우
            if (health <= 0)
            {
                //Debug.Log($"Enemy의 skillPoint : {skillPoint}");
                gameObject.SetActive(false); // 비활성화
                ObjectManager.Instance.SpawnFromPool("DestroyFX", transform.position, transform.rotation); // 폭팔 이펙트 생성
                GameManager.Instance.SkillPoint += enemyPoint; // 기체의 스킬 포인트 획득
            }
        }
    }

    // 피격시 스프라이트 복구
    private void Attacked()
    {
        spriteRenderer.sprite = sprites[0]; // 원상태 이미지
    }

    // 오브젝트 풀링의 적 총알 타입에 맞게 생성
    private void Shooting()
    {
        switch (enemyType)
        {
            case "EBL":
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                break;
            case "EBM":
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                break;
            case "EBS":
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                break;
        }

        startTime = 0; // 경과 시간 초기화

    }
}
