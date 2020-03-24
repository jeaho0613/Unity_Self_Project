using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public bool isWalk = true;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 좌,우 이동키를 받음
        float movePoint = Input.GetAxisRaw("Horizontal");

        // 좌,우를 판단하여 Translate로 이동
        if (movePoint > 0 || movePoint < 0)
        {
            transform.Translate(new Vector2(movePoint * moveSpeed * Time.deltaTime, 0f));
            if (movePoint < 0) // 좌측으로 움직일 경우 x반전
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;

            // walk animation 실행
            animator.SetBool("isWalk", true); 
        }
        else // 키입력이 없을 경우
        {
            // walk animation 종료
            animator.SetBool("isWalk", false);
        }
    }
}
