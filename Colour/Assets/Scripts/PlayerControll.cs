using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Range(1f, 10f)]
    private float moveSpeed;
    [SerializeField]
    private string currentColor;
    [SerializeField]
    private Color[] Colors;
    [SerializeField]
    private SpriteRenderer playerRenderer;

    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PlayerMove();

        if(Input.GetMouseButtonDown(0))
        {
            ChageColor();
        }
    }

    // 플레이어 이동 로직
    private void PlayerMove()
    {
        float xInput = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float yInput = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(xInput, yInput, 0);
    }
    
    // 랜덤 컬러 변수
    // - 0 ~ 3의 숫자를 반환, currentColor의 이름값 변경
    private void ChageColor()
    {
        int random = Random.Range(0, 4);
        switch(random)
        {
            case 0:
                currentColor = "Red";
                playerRenderer.color = Colors[random];
                break;
            case 1:
                currentColor = "Blue";
                playerRenderer.color = Colors[random];
                break;
            case 2:
                currentColor = "Green";
                playerRenderer.color = Colors[random];
                break;
            case 3:
                currentColor = "Yellow";
                playerRenderer.color = Colors[random];
                break;
        }
    }
}
