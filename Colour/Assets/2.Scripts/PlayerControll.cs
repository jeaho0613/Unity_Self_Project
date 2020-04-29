using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviour
{
    public bool isDie = false; // 사망 체크
    public bool isRespawn = false; // 리스폰 체크
    public bool isSkill = false; // 스킬 사용 체크

    public string currentColor; // 현재 색깔
    [Range(1f, 10f)]
    public float moveSpeed; // 스피드 
    [Range(0f, 1f)]
    public float shootingTime; // 발사 간격
    public Texture2D[] sprites; // player 이미지
    public GameObject[] bulletPrefabs; // 총알 프리팹

    private Material playerMaterial; // player Material
    private Animator playerAnimator; // player Animetor
    private CircleCollider2D playerBoxCollider; // player boxCollider
    private AudioSource playerAudioSource; // player AudioSource
    private SpriteRenderer PlayerSpriteRenderer; // player SpriteRenderer

    // 플레이어 Color enum 변수
    private enum EnumColors
    {
        Red,
        Green,
        Blue,
        Yellow
    }
    private EnumColors enumColors; // Color Enum 변수
    private float shootingCool = 0; // 발사 시간

    private void Awake()
    {
        playerBoxCollider = GetComponent<CircleCollider2D>(); // Boxcollider2D 초기화
        playerAnimator = GetComponent<Animator>(); // Animator 초기화
        playerMaterial = GetComponent<SpriteRenderer>().material; // Material 초기화
        playerAudioSource = GetComponent<AudioSource>(); // Audio 초기화
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 초기화
        ChageColor(); // 최초실행시 player 색상 초기화
    }

    private void Update()
    {
        // 게임이 시작하지 않았다면
        if (!GameManager.Instance.IsStart)
        {
            GameStart();
            return;
        }

        else if ((!isDie || isRespawn) && !GameManager.Instance.isWin) // !사망중이 아닐 때 리스폰 중일 때
        {
            PlayerMove(); // 이동 
            PlayerClick(); // 색 변경
            PlayerShooting(); // 총 발사
            return;
        }

        else if (isDie || GameManager.Instance.isWin) // 사망중 이거나 게임에서 이겼을 때
        {
            GameReStart();
            return;
        }


    }

    #region PlayerMove() 플레이어 이동 로직
    private void PlayerMove()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        transform.Translate(xInput * moveSpeed * Time.deltaTime, // x축 입력
                            yInput * moveSpeed * Time.deltaTime, // y축 입력
                            0); // z축 입력

        // 화면 제한 로직
        // - min, max에 카메라 끝점(왼쪽하단, 오른쪽 상단)을 vector2 값으로 받아옴
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0.07f));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 playerPos = transform.position;

        // Clamp (최소,최대 값을 정해줌)
        playerPos.x = Mathf.Clamp(playerPos.x, min.x, max.x);
        playerPos.y = Mathf.Clamp(playerPos.y, min.y, max.y);

        // 그 값을 플레이어 postion 값에 넣어줌
        transform.position = playerPos;

        // palyer 움직임 애니메이션 
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            playerAnimator.SetInteger("Input", (int)xInput);
        }
    }
    #endregion

    #region PlayerClick() 키에 따른 색 변경 로직
    // - 각 특정키 입력에 따라서 enumColors를 교체해줌.
    // - ChageColor 함수를 통해 enumColors와 비교하여 색 변경
    // - 각 색에 따른 능력치 변환
    private void PlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && (enumColors != EnumColors.Red))
        {
            playerAudioSource.PlayOneShot(SoundManager.Instance.PlayerSounds[1]); // 소리 출력
            enumColors = EnumColors.Red; // 색 변경
            ChageColor(); // 색 변경 실행
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && (enumColors != EnumColors.Green))
        {
            playerAudioSource.PlayOneShot(SoundManager.Instance.PlayerSounds[1]);
            enumColors = EnumColors.Green; // 색 변경
            ChageColor(); // 색 변경
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && (enumColors != EnumColors.Blue))
        {
            playerAudioSource.PlayOneShot(SoundManager.Instance.PlayerSounds[1]);
            enumColors = EnumColors.Blue; // 색 변경
            ChageColor(); // 색 변경
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && (enumColors != EnumColors.Yellow) && (GameManager.Instance.SkillCount > 0) && !isSkill)
        {
            isSkill = true;
            StartCoroutine(PlayerSkill());
            playerAudioSource.PlayOneShot(SoundManager.Instance.PlayerSounds[1]); // 소리 출력
            GameManager.Instance.SkillCount--; // 스킬 카운트 감소
            enumColors = EnumColors.Yellow; // 색 변경
            ChageColor(); // 색 변경
        }
       
    }
    #endregion

    #region PlayerShooting() 플레이어 발사 로직
    // - enumColor에 따라서 총알 색 변경
    // - 오브젝트 풀 사용
    private void PlayerShooting()
    {
        shootingCool += Time.deltaTime;
        if (shootingCool > shootingTime)
        {
            // 총알 발사 소리
            playerAudioSource.PlayOneShot(SoundManager.Instance.PlayerSounds[0]);

            // 오브젝트 풀 사용
            ObjectManager.Instance.SpawnFromPool(currentColor, transform.position, Quaternion.identity);

            // cool 초기화
            shootingCool = 0f;
        }
    }
    #endregion

    #region ChageColor() 컬러 변환 로직
    // - 키 입력에 따라 enum값을 변경하여 색을 변경함
    // - 쉐이더 그래프를 이용한 텍스쳐 변경
    private void ChageColor()
    {
        // enumColor에 따라서 색 변경
        int num = (int)enumColors;

        switch (num)
        {
            case 0:
                moveSpeed = 3f; // 능력치 변경
                shootingTime = 0.05f; // 능력치 변경
                currentColor = "Red";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                break;
            case 1:
                moveSpeed = 0.7f; // 능력치 변경
                shootingTime = 0.1f; // 능력치 변경
                currentColor = "Green";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                break;
            case 2:
                moveSpeed = 7f; // 능력치 변경
                shootingTime = 0.2f; // 능력치 변경
                currentColor = "Blue";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                break;
            case 3:
                moveSpeed = 7f; // 능력치 변경
                shootingTime = 0.02f; // 능력치 변경
                currentColor = "Yellow";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                playerBoxCollider.enabled = false;
                break;
        }
    }
    #endregion

    #region playerDie() 플레이어 사망 로직
    public void PlayerDie()
    {
        isDie = true; // 사망 체크
        playerBoxCollider.enabled = false; // 충돌 비활성화
        PlayerSpriteRenderer.color = Color.clear; // player 색 제거
        GameManager.Instance.Life--; // 목숨 감소
        ObjectManager.Instance.SpawnFromPool("DestroyFX", transform.position, transform.rotation); // 폭파 애니메이션 생성
        playerAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[(int)enumColors + 2]); // 사망 효과음 발생 
        if (GameManager.Instance.Life == 0)
        {
            return; // life가 0이면 중지
        }
        Invoke("PlayerRespawn", 2); // 사망한뒤 2초뒤 생성
    }
    #endregion

    #region PlayerRespawn() 플레이어 리스폰 로직
    private void PlayerRespawn()
    {
        isDie = false; // 사망 플래그 해제
        isRespawn = true; // 리스폰 중
        PlayerSpriteRenderer.color = new Color(1, 1, 1, 0.5f); // 불투명 색으로 
        gameObject.SetActive(true); // 활성화
        gameObject.transform.position = new Vector2(0, -4); // 위치값 변경
        Invoke("PlayerRespawnColor", 2); // 생성 후 2초간 무적
    }
    #endregion

    #region PlayerRespawnColor() 플레이어 색 복귀 로직
    private void PlayerRespawnColor()
    {
        isRespawn = false; // 리스폰 해제
        playerBoxCollider.enabled = true; // 충돌 비활성화
        PlayerSpriteRenderer.color = Color.white; // 색 복귀
    }
    #endregion

    #region PlayerSkill() 플레이어 스킬 로직
    IEnumerator PlayerSkill()
    {
        EnumColors saveNum = enumColors; // 스킬 사용전 원래 color 저장
        yield return new WaitForSeconds(2f); // 2초 대기
        isSkill = false;
        enumColors = saveNum; // 사용전 color로 돌려줌
        ChageColor(); // 색 변경
        playerBoxCollider.enabled = true;
    }
    #endregion

    #region OnTriggerEnter2D() 플레이어 충돌 판정
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적 총알 || 적 기체에 충돌시
        if ((other.tag == "EBullet" || other.tag == "Enemy" || other.tag == "Razer") && !isDie)
        {
            PlayerDie(); // player Die
        }
    }
    #endregion

    #region GameReStart() 게임 restart 로직
    private void GameReStart()
    {
        if (Input.GetKeyDown(KeyCode.R) && (GameManager.Instance.isLose || GameManager.Instance.isWin))
        {
            Debug.Log("R버튼 클릭");
            SceneManager.LoadScene(1);
        }
    }
    #endregion

    #region GameStart() 게임 처음 스타트 로직
    private void GameStart()
    {
        if (Input.GetKeyDown(KeyCode.R) && (GameManager.Instance.IsStart == false))
        {
            //Debug.Log("게임 시작!");
            GameManager.Instance.IsStart = true;
        }
    }
    #endregion

}