using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region Text파일 읽는데 필요한 변수
    // Spawn 구조체
    public class Spawn
    {
        public float delay; // 생성시간
        public string type; // 생성타입
        public int point; // 생성위치
    }
    public List<Spawn> spawnList; // Spawn구조체 리스트
    #endregion

    #region [Life 프로퍼티]

    [SerializeField]
    private int life; // 플레이어 수명
    public int Life
    {
        get
        {
            return life;
        }

        set
        {
            // 라이프가 0 이하이면
            if (value < 0)
            {
                
                life = 0; // 라이프는 0으로 초기화
                return; // 리턴
            }
            // 라이프가 0 이상이면

            life = value; // 라이프 값 변경
            UpdateLifeUI(); // Life UI 실행
        }
    }
    #endregion

    #region [skillPoint 프로퍼티]

    [SerializeField]
    private float skillPoint; // 스킬 포인트
    public float SkillPoint
    {
        get
        {
            return skillPoint;
        }

        set
        {
            // skillCount가 3이상이면
            if (skillCount >= 3)
            {
                skillPoint = 0; // skillPoint는 0으로 초기화
                skillPowerBar.fillAmount = skillPoint; // 게이지 업데이트
                return; // 리턴
            }

            // skillCount가 3보다 작으면

            skillPoint = value; // skillPoint값 변경
            UpdateSkillPointUI(); // PointUI 실행
        }
    }
    #endregion

    #region [skillCount 프로퍼티]

    [SerializeField]
    private float skillCount; // 스킬 사용횟수
    public float SkillCount
    {
        get
        {
            return skillCount;
        }
        set
        {
            // skillCount가 3이상 크면
            if (value >= 3)
            {
                skillCount = 3; // skillCount는 3으로 초기화
                return; // 리턴
            }
            // skillCount가 3보다 작으면
            
            skillCount = value; // skillCount 값 변경
            UpdateSkillCountUI(); // Skill UI 실행
        }
    }
    #endregion

    #region [isStart 프로퍼티]
    [SerializeField]
    private bool isStart = false;
    public bool IsStart
    {
        get
        {
            return isStart;
        }
        set
        {
            if (value)
            {
                //Debug.Log("if value의 값 :" + value);
                TextSetActive(value);
            }
            else
            {
                //Debug.Log("else value의 값 :" + value);
                TextSetActive(value);
            }
        }
    }
    #endregion

    public static GameManager Instance; // 싱글톤
    public GameObject[] lifeGameObjects; // 라이프 오브젝트
    public GameObject[] skillPowerGameObjects; // 스킬 오브젝트
    public Image skillPowerBar; // 스킬 바
    public Animator endGameAnimation; // 게임 끝 텍스트 에니메이션
    public Text startGameText; // 게임 스타트 텍스트
    public GameObject enemySpawner; // 적 소환 오브젝트
    public bool isBoss = false; // 포스전 체크
    public bool isLose = false; // 플레이어 Game End 체크
    public bool isWin = false; // 플레이어 Game Win 체크

    // 초기화
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Screen.SetResolution(540, 900, false); // 화면 해상도 고정
        spawnList = new List<Spawn>(); // spawnList 초기화
        ReadSpawnFile(); // Text 파일 

        life = 3; // 목숨 3개로 초기화
        skillPoint = 0; // 스킬 포인트 초기화
        skillCount = 0; // 스킬 횟수 초기화
    }

    #region Text파일 읽기
    void ReadSpawnFile()
    {
        spawnList.Clear(); // 초기화

        // Resources폴더의 stage 0 텍스트 읽기
        TextAsset textFile = Resources.Load("stage 0") as TextAsset;
        // StringReader로 파일 열기
        StringReader stringReader = new StringReader(textFile.text);

        // 마지막 줄을 읽을 때의 예외처리
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            //Debug.Log(line); //읽은값 출력

            // 마지막줄 즉 null 값이면 정지
            if (line == null)
            {
                break;
            }

            Spawn spawnData = new Spawn(); // 생성자
            spawnData.delay = float.Parse(line.Split(',')[0]); // 텍스트 첫번째 값 저장
            spawnData.type = line.Split(',')[1]; // 텍스트 두번째 값 저장
            spawnData.point = int.Parse(line.Split(',')[2]); // 텍스트 세번째 값 저장
            spawnList.Add(spawnData); // List에 spawnData 저장
        }
        stringReader.Close(); // StringReader 닫아주기
    }
    #endregion

    #region UpdateLifeUI() 라이프 UI 업데이트 로직
    private void UpdateLifeUI()
    {
        // 각 life의 값에 따른 UI 변경
        switch (life)
        {
            case 3:
                lifeGameObjects[0].SetActive(true);
                lifeGameObjects[1].SetActive(true);
                lifeGameObjects[2].SetActive(true);
                break;
            case 2:
                lifeGameObjects[0].SetActive(true);
                lifeGameObjects[1].SetActive(true);
                lifeGameObjects[2].SetActive(false);
                break;
            case 1:
                lifeGameObjects[0].SetActive(true);
                lifeGameObjects[1].SetActive(false);
                lifeGameObjects[2].SetActive(false);
                break;
            case 0:
                lifeGameObjects[0].SetActive(false);
                lifeGameObjects[1].SetActive(false);
                lifeGameObjects[2].SetActive(false);

                isLose = true; // 게임 end를 체크
                SoundManager.Instance.GetComponent<AudioSource>().Stop(); // 배경음 정지
                GameEnd(1); // End 사운드 출력
                break;
               
        }
    }
    #endregion

    #region UpdateSkillPointUI() 스킬 포인트 UI 업데이트
    private void UpdateSkillPointUI()
    {
        // skillPoint가 1이상 이면
        // - 스킬 게이지가 1이 최대이므로
        if (skillPoint >= 1)
        {
            skillCount++; // skillCount 증가
            // 증가 시킨 skillCount의 값이 3이면
            if(skillCount == 3)
            {
                skillPoint = 0; // skillPoint는 0으로 초기화
                skillPowerBar.fillAmount = skillPoint; // 게이지 변경
                UpdateSkillCountUI(); // skill UI 실행
                return; // 리턴
            }
            // skillCount가 3보다 작으면
            skillPoint -= 1; // skillPoint에 1을 빼줌
            UpdateSkillCountUI(); // skill UI 실행
        }
        // skillPoint가 1보다 작을 때
        skillPowerBar.fillAmount = skillPoint; // 스킬 게이지 변경
    }

    #endregion

    #region UpdateSkillCountUI() 스킬 카운트 UI 업데이트
    private void UpdateSkillCountUI()
    {
        switch (skillCount)
        {
            case 3:
                skillPowerGameObjects[0].SetActive(true);
                skillPowerGameObjects[1].SetActive(true);
                skillPowerGameObjects[2].SetActive(true);
                break;
            case 2:
                skillPowerGameObjects[0].SetActive(true);
                skillPowerGameObjects[1].SetActive(true);
                skillPowerGameObjects[2].SetActive(false);
                break;
            case 1:
                skillPowerGameObjects[0].SetActive(true);
                skillPowerGameObjects[1].SetActive(false);
                skillPowerGameObjects[2].SetActive(false);
                break;
            case 0:
                skillPowerGameObjects[0].SetActive(false);
                skillPowerGameObjects[1].SetActive(false);
                skillPowerGameObjects[2].SetActive(false);
                // 게임 오버 UI
                break;
        }
    }
    #endregion

    #region GameEnd(int num) 게임 종료 로직 *num은 출력할 사운드
    public void GameEnd(int num)
    {
        // 게임 오버 UI
        //Debug.Log("life end");
        SoundManager.Instance.GetComponent<AudioSource>()
            .PlayOneShot(SoundManager.Instance.FXSounds[num]); // End Sound 출력
        endGameAnimation.SetTrigger("isEnd"); // 엔드 게임 이미지 출력
    }
    #endregion

    #region TextSetActive() 처음 게임 스타트 
    private void TextSetActive(bool value)
    {
        startGameText.gameObject.SetActive(!value); // 프로퍼티에 상태에 따라 TextUI 활성화
        enemySpawner.SetActive(value); // 게임 시작을 하면 적 기체 스포너 활성화
        isStart = value; // isStart 값 변경
    }
    #endregion

}
