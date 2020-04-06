using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTest; // 테스트용 무적

    public float jumpPower; // 점프 힘
    public float savePower; // 세이브 점프 힘
    public string currentColor; // 현재 색깔
    public Color Blue; // 각 설정된 색깔
    public Color Yellow; // 각 설정된 색깔
    public Color Red; // 각 설정된 색깔
    public Color Purple; // 각 설정된 색깔
    public GameObject[] changeColors; // chagecolor 오브젝트 배열
    public StageManager stageManager; // StageManager 오브젝트 
    public AudioClip[] audioClips; // 플레이어 사운드 클립

    private AudioSource playerAudioSource; // 플레이어 AudioSource
    private Rigidbody2D playerRigi; // 플레이어 rigidbody
    private SpriteRenderer playerRenderer; // 플레이어 SpriteRenderer

    // 참조 초기화
    private void Awake()
    {
        playerRigi = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    // player 변수 초기화
    void Start()
    {
        RandomColor(); // 색 지정
        stageManager.SetPostion(); // 위치값 로드
        stageManager.StageActive(); // 스테이지 로드
    }

    void Update()
    {
        // 마우스 왼쪽 클릭 || 스페이스바 누를 때 동작
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            playerAudioSource.PlayOneShot(audioClips[1]); // 점프 소리
            playerRigi.velocity = Vector2.up * jumpPower; // velocity 위쪽 방향으로
        }
    }

    // 플레이어가 오브젝트에 부딪칠 때 동작
    private void OnTriggerEnter2D(Collider2D other)
    {
        // changeColor 오브젝트 동작
        if (other.tag == "Change")
        {
            playerAudioSource.PlayOneShot(audioClips[0]); // 소리 출력
            RandomColor(); // 색을 변경
            other.gameObject.SetActive(false); // 오브젝트 비활성화
            return;
        }

        // star(별) 오브젝트 동작
        if (other.tag == "Star")
        {
            playerAudioSource.PlayOneShot(audioClips[2]); // 소리 출력
            GameManager.Instance.LoadPoint += 1; // Score를 1올려줌
            stageManager.StageActive(); // 다음 stage 로드
            stageManager.SetPostion(); // 다음 stage 포인트로 이동
            other.gameObject.SetActive(false); // 오브젝트 비활성화
            return;
        }

        // 테스트용 무적
        if (isTest)
        {
            return;
        }

        // 그 이외(장애물)에 부딪쳤을 때 동작
        if (currentColor != other.tag)
        {
            playerAudioSource.PlayOneShot(audioClips[3]); // 소리 출력
            RandomColor(); // 색 다시 지정
            stageManager.SetPostion(); // 장애물에 부딪쳤을 경우 postion으로 돌아감
            playerRigi.velocity = Vector2.zero; // velocity 초기화
            stageManager.ChangeColorActive(); // chageColor오브젝트 재 활성화
            return;
        }
    }

    // 랜덤 색 변수
    public void RandomColor()
    {
        int index = Random.Range(0, 4);

        switch (index)
        {
            case 0:
                currentColor = "Blue";
                playerRenderer.color = Blue;
                break;
            case 1:
                currentColor = "Yellow";
                playerRenderer.color = Yellow;
                break;
            case 2:
                currentColor = "Red";
                playerRenderer.color = Red;
                break;
            case 3:
                currentColor = "Purple";
                playerRenderer.color = Purple;
                break;
        }
    }
}
