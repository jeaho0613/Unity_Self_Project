using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [Range(1f, 10f)]
    public float endSpeed; // 도착지점에 도달되는 시간
    [Range(0f, 5f)]
    public float shootingDelay; // 총알 간격
    public float health; // 체력
    public float enemyPoint; // 파괴 스킬 포인트
    public Sprite[] sprites; // 피격 효과시 교체될 이미지
    public string enemyType; // 현재 기체 타입

    private float savehealth; // 재 생성될때 저장 체력값
    private SpriteRenderer spriteRenderer; // enemy 기체 이미지렌더러
    private float startTime; // 총알 발사 주기
    private HealthBar healthBar;

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
        // 기체 타입이 EnemyBL이면 hp bar 초기화
        if (gameObject.name == "EnemyBL" || gameObject.name == "EnemyBS")
        {
            healthBar = transform.GetComponentInChildren<HealthBar>(); // hp bar 초기화
            healthBar.SetMaxHealth((int)health);
        }; // 보스 체력바 최대 값 초기화
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
            if (gameObject.name == "EnemyBL"|| gameObject.name == "EnemyBS")
            {
                healthBar.SetHealth((int)health); 
            }; // EnemyBL 타입 기체면 HP Bar 감소
            Invoke("Attacked", 0.1f); // 이미지 복원

            // 만약 체력이 0보다 작아질 경우
            if (health <= 0)
            {
                //Debug.Log($"Enemy의 skillPoint : {skillPoint}");
                if (other.name != "BulletY")
                {
                    GameManager.Instance.SkillPoint += enemyPoint; 
                };
                gameObject.SetActive(false); // 비활성화
                var fxDamme = ObjectManager.Instance.SpawnFromPool("DestroyFX", transform.position, transform.rotation); // 폭팔 이펙트 생성
                FxSize(fxDamme);
                
            }
        }
    }

    #region Attacked() 피격 이미지 복구
    private void Attacked()
    {
        spriteRenderer.sprite = sprites[0]; // 원상태 이미지
    }
    #endregion

    #region Shooting() 오브젝트 풀링의 적 총알 타입에 맞게 생성
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
            case "EBBL":
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(0.5f, -0.5f, 0), Quaternion.identity);
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(-0.5f, -0.5f, 0), Quaternion.identity);
                break;
            case "EBBS":
                ObjectManager.Instance.SpawnFromPool(enemyType, transform.position + new Vector3(0.5f, -0.5f, 0), Quaternion.identity);
                break;
        }
        startTime = 0; // 경과 시간 초기화
    }
    #endregion

    #region FxSize() 폭팔 이펙트 적 기체 크기에 따른 사이즈 변ㄴ경
    private void FxSize(GameObject damme)
    {
        switch (enemyType)
        {
            case "EBL":
                damme.transform.localScale = new Vector2(2, 2);
                break;

            case "EBM":
                damme.transform.localScale = new Vector2(1.5f, 1.5f);
                break;

            case "EBS":
                damme.transform.localScale = new Vector2(1, 1);
                break;
            case "EBBS":
                damme.transform.localScale = new Vector2(1, 1);
                break;
            case "EBBL":
                damme.transform.localScale = new Vector2(2, 2);
                break;
        }
    }
    #endregion
}
