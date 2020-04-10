using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTest; // 테스트 체크용
    public bool isFlash; // Flash 스킬 사용중 체크용 
    public bool isTimeStop; // timestop 스킬 사용중 체크용

    public float jumpPower; // 점프 힘
    public float savePower; // 세이브 점프 힘
    public string currentColor; // 현재 색깔
    public float saveTime; // 경과시간 변수
    public Color Blue; // 각 설정된 색깔
    public Color Yellow; // 각 설정된 색깔
    public Color Red; // 각 설정된 색깔
    public Color Purple; // 각 설정된 색깔
    public GameObject[] changeColors; // chagecolor 오브젝트 배열
    public StageManager stageManager; // StageManager 오브젝트 
    public SoundManger soundManger;// 사운드 매니저 

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
        stageManager.setPostion(); // 위치값 로드
        stageManager.StageActive(); // 스테이지 로드
        RandomColor(); // 색 지정
    }

    void Update()
    {
        PlayerMove(); // 플레이어 움직임
    }

    public void PlayerMove()
    {
        // 마우스 왼쪽 클릭 || 스페이스바 누를 때 동작
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            playerAudioSource.PlayOneShot(soundManger.fxClips[1]); // 점프 소리
            playerRigi.velocity = Vector2.up * jumpPower; // velocity 위쪽 방향으로
        }
        // flsh 스킬
        // - 첫번째 stage를 통과후
        if (Input.GetKeyDown(KeyCode.Q) && (GameManager.Instance.LoadPoint > 0) && !isFlash)
        {
            isFlash = true;
            transform.position += new Vector3(0, 3, 0);
            playerRigi.velocity = Vector3.zero;
            StartCoroutine(delay(1f));
            isFlash = false;
        }
        // timestop 스킬 사운드
        // - 두번째 stage를 통과후
        if (Input.GetKeyDown(KeyCode.W) && (GameManager.Instance.LoadPoint > 1) && !isTimeStop)
        {
            isTimeStop = true;
            playerAudioSource.PlayOneShot(soundManger.fxClips[4]); // 점프 소리
            StartCoroutine(delay(3f));
            isTimeStop = false;
        }
    }

    // 플레이어가 오브젝트에 부딪칠 때 동작
    private void OnTriggerEnter2D(Collider2D other)
    {
        // changeColor 오브젝트 동작
        if (other.tag == "Change")
        {
            playerAudioSource.PlayOneShot(soundManger.fxClips[0]); // 소리 출력
            RandomColor(); // 색을 변경
            other.gameObject.SetActive(false); // 오브젝트 비활성화
            return;
        }

        // star(별) 오브젝트 동작
        if (other.tag == "Star")
        {
            playerAudioSource.PlayOneShot(soundManger.fxClips[2]); // 소리 출력
            GameManager.Instance.LoadPoint += 1; // Score를 1올려줌
            GameManager.Instance.starActive[GameManager.Instance.LoadPoint - 1].SetActive(true); // 별 이미지 활성화
            stageManager.StageActive(); // 다음 stage 로드
            stageManager.Postion(); // 다음 stage 포인트로 이동
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
            playerAudioSource.PlayOneShot(soundManger.fxClips[3]); // 소리 출력
            RandomColor(); // 색 다시 지정
            stageManager.Postion(); // 장애물에 부딪쳤을 경우 postion으로 돌아감
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

    // 딜레이 코루틴
    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
