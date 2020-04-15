using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public string currentColor; // 현재 색깔
    [Range(1f, 10f)]
    public float moveSpeed; // 스피드 
    [Range(0f, 1f)]
    public float shootingTime; // 발사 간격
    public Texture2D[] sprites; // player 이미지
    public GameObject[] bulletPrefabs; // 총알 프리팹

    private enum EnumColors
    {
        Red,
        Green,
        Blue,
        Yellow
    } 
    private EnumColors enumColors; // Color Enum 변수

    private Material playerMaterial; // player 머테리얼
    private Animator playerAnimator; // player 애니메이션
    private BoxCollider2D playerBoxCollider; // player box콜라이더

    private float shootingCool = 0; // 발사 시간


    private void Awake()
    {
        playerBoxCollider = GetComponent<BoxCollider2D>(); // player Boxcollider2d
        playerAnimator = GetComponent<Animator>(); // player Animator 
        playerMaterial = GetComponent<SpriteRenderer>().material; // player Material
        ChageColor(); // 최초실행시 player 색상 초기화
    }

    private void Update()
    {
        PlayerMove();
        PlayerClick();
        PlayerShooting();
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
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        //Debug.Log("min값 : "+min);
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        //Debug.Log("max값 : " + max);
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enumColors = EnumColors.Red;
            moveSpeed = 3f;
            shootingTime = 0.05f;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enumColors = EnumColors.Green;
            moveSpeed = 0.7f;
            shootingTime = 0.1f;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enumColors = EnumColors.Blue;
            moveSpeed = 7f;
            shootingTime = 0.2f;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            enumColors = EnumColors.Yellow;
            moveSpeed = 7f;
            shootingTime = 0.02f;
            ChageColor();
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
            // 오브젝트 풀 사용
            // - currentColor에 따른 총알 변경
            ObjectManager.Instance.SpawnFromPool(currentColor, transform.position, Quaternion.identity); // 오류가 나는 부분
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
                currentColor = "Red";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                playerBoxCollider.enabled = true;
                break;
            case 1:
                currentColor = "Green";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                playerBoxCollider.enabled = true;
                break;
            case 2:
                currentColor = "Blue";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                playerBoxCollider.enabled = true;
                break;
            case 3:
                currentColor = "Yellow";
                playerMaterial.SetTexture("_SubTex", sprites[num]); // 텍스쳐 변경
                playerBoxCollider.enabled = false;
                break;
        }
    }
    #endregion
}