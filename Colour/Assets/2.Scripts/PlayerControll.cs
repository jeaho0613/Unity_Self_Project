using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Range(1f, 10f)]
    public float moveSpeed; // 스피드 
    public Color[] Colors; // 색깔
    public string currentColor; // 현재 색깔

    public GameObject[] bulletPrefabs; // 총알 프리팹
    [Range(0f,1f)]
    public float shootingTime; // 발사 간격
    public float shootingCool = 0; // 발사 시간

    private enum EnumColors
    {
        White,
        Red,
        Blue,
        Green,
        Yellow
    } // 색깔을 표시할 enum변수
    private EnumColors enumColors;
    private SpriteRenderer playerRenderer; // player 색변경 Renderer


    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PlayerMove();
        PlayerClick();
        PlayerShooting();
    }

    // 플레이어 이동 로직
    private void PlayerMove()
    {
        float xInput = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float yInput = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(xInput, yInput, 0);

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
    }

    // 키에 따른 색 변경 로직
    // - 각 특정키 입력에 따라서 enumColors를 교체해줌.
    // - ChageColor 함수를 통해 enumColors와 비교하여 색 변경
    private void PlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            enumColors = EnumColors.White;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enumColors = EnumColors.Red;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enumColors = EnumColors.Blue;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enumColors = EnumColors.Green;
            ChageColor();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            enumColors = EnumColors.Yellow;
            ChageColor();
        }
    }

    // 플레이어 발사 로직
    // - enumColor에 따라서 총알 색 변경
    private void PlayerShooting()
    {

        shootingCool += Time.deltaTime;
        if (shootingCool > shootingTime)
        {
            Instantiate(bulletPrefabs[(int)enumColors], transform.position, transform.rotation);
            shootingCool = 0f;
        }
    }

    // 컬러 변환 로직
    // - 키 입력에 따라 enum값을 변경하여 색을 변경함
    private void ChageColor()
    {
        // enumColor에 따라서 색 변경
        int num = (int)enumColors;

        switch (num)
        {
            case 0:
                currentColor = "White";
                playerRenderer.color = Colors[num];
                break;
            case 1:
                currentColor = "Red";
                playerRenderer.color = Colors[num];
                break;
            case 2:
                currentColor = "Blue";
                playerRenderer.color = Colors[num];
                break;
            case 3:
                currentColor = "Green";
                playerRenderer.color = Colors[num];
                break;
            case 4:
                currentColor = "Yellow";
                playerRenderer.color = Colors[num];
                break;
        }
    }

}

