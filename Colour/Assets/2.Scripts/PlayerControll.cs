using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Range(1f,10f)]
    public float moveSpeed; // 스피드 
    public Color[] Colors; // 색깔
    public string currentColor; // 현재 색깔
    public GameObject bulletPrefabs; // 총알 프리팹

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
    }

    // 플레이어 이동 로직
    private void PlayerMove()
    {
        float xInput = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float yInput = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(xInput, yInput, 0);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp01(viewPos.x);
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

    // 충돌했을 때 로직
    
}
