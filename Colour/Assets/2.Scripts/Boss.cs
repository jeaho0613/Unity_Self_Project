using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class Boss : MonoBehaviour
{
    public bool isNextPage = false; // 페이지 변환 체크
    public bool isRazer = false; // 레이저 스킬 시작 중
    public float health; // 현재 체력
    public int maxCountCheck; // 최대 발사 체크
    public int pattenCount; // 발사 횟수
    public int currentPattern; // 현재 패턴
    public int currentPage; // 현재 보스 페이지
    public Transform[] shootPoints; // 총 발사 포인트
    public int[] maxPatternCounts; // 각 패턴발사 수
    public Vector3 saveSize; // 사이즈 세이브

    private CapsuleCollider2D bossCapsuleCollider2D; // Boss 콜라이더
    private SpriteRenderer bossSpriteRenderer; // Boss spriteRenderer
    private Transform playerPostion; // 플레이어 위치값
    private BarSpawner barSpawner; // 바 스폰을 하기 위한 변수
    private LineRenderer bossLineRenderer_1; // 라인 렌더러
    private LineRenderer bossLineRenderer_2; // 라인 렌더러
    private EdgeCollider2D edgeCollider_1; // 엣지 콜라이더
    private EdgeCollider2D edgeCollider_2; // 엣지 콜라이더
    private AudioSource bossAudioSource; // 보스 오디오
    private Animator bossAnimator; // 보스 애니메이터
    private HealthBar healthBar; // HP Bar 
    private float[] screenWidths = new float[9]; // 화면 비율값에 대한 x값
    private float mainCamWidth; // 화면 가로 길이

    private Color2 resetColor = new Color2(new Color(1, 0, 0, 0.5f), new Color(1, 0, 0, 0.5f)); // 레이져 컬러 셋
    private Color2 startColor = new Color2(Color.white, Color.white); // 레이져 컬러 셋
    private Color2 endColor = new Color2(Color.clear, Color.clear); // 레이져 컬러 셋

    // Move 사용 시퀀스
    private Sequence loopMoveSequence1; // 루프 무브 
    private Sequence loopMoveSequence2; // 루프 무브 
    private Sequence loopMoveSequence3; // 루프 무브 
    private Sequence bossSkill_1; // skill 1
    private Sequence bossSkill_5; // skill 1
    private Sequence respawnBossSequence; // 보스 리스폰 움직임


    private Sequence _LoopMoveSequence_1; // page 1 
    private Sequence _LoopMoveSequence_2; // page 2
    private Sequence _LoopMoveSequence_3; // page 3 
    private Sequence _BossSkill_1; // skill 1
    private Sequence _BossSkill_5; // skill 1
    private Sequence _RespawnBossSequence; // 리스폰 움직임

    private void Awake()
    {
        ScreenMath(); // 스크린 비율 초기화

        bossCapsuleCollider2D = GetComponent<CapsuleCollider2D>(); // 초기화
        bossSpriteRenderer = GetComponent<SpriteRenderer>(); // 초기화
        barSpawner = GetComponent<BarSpawner>(); // 초기화
        playerPostion = FindObjectOfType<PlayerControll>().transform; // 플레이어 위치 받기
        bossAudioSource = GetComponent<AudioSource>(); // 초기화
        bossAnimator = GetComponent<Animator>(); // 초기화

        bossLineRenderer_1 = transform.GetChild(0).GetComponent<LineRenderer>(); // 라인 렌더러 초기화
        bossLineRenderer_2 = transform.GetChild(1).GetComponent<LineRenderer>(); // 라인 렌더러 초기화

        healthBar = transform.GetComponentInChildren<HealthBar>(); // hp bar 초기화

        edgeCollider_1 = transform.GetChild(0).GetComponent<EdgeCollider2D>(); // Lager edgeCollider 초기화
        edgeCollider_2 = transform.GetChild(1).GetComponent<EdgeCollider2D>(); // Lager edgeCollider 초기화

        barSpawner.enabled = false; // 초기 바 스폰 제거
        currentPage = 1; // 페이지 초기화 
        currentPattern = 1; // 페턴 초기화
        isNextPage = true; // 처음 소환시 탄 발사 정지
        saveSize = transform.localScale; // 처음 초기화
    }

    private void Start()
    {
        #region DOTween Sequence 초기화

        _LoopMoveSequence_1 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2, 3f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2, 3f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        loopMoveSequence1 = _LoopMoveSequence_1;

        _LoopMoveSequence_2 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        loopMoveSequence2 = _LoopMoveSequence_2;

        _LoopMoveSequence_3 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2.4f, 0.7f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2.4f, 0.7f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        loopMoveSequence3 = _LoopMoveSequence_3;

        _RespawnBossSequence = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMove(new Vector3(0, 3, 0), 3f)).SetEase(Ease.Linear) // 시작점 조정
                                      .Append(transform.DOMoveX(-2, 1.5f)).SetEase(Ease.Linear) // 왼쪽으로 위치조정
                                      .OnComplete(() => // 도착시
                                      {
                                          SequenceUpdate(); // 시퀀스 초기화
                                          loopMoveSequence2.Restart(); // 좌우 루프 트윈 시작
                                      });
        respawnBossSequence = _RespawnBossSequence;

        // Boss skill_1 시퀀스
        _BossSkill_1 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMove(new Vector3(0, 8, 0), 1f))
                                      .Append(bossSpriteRenderer.DOColor(new Color(0, 0, 0, 0.5f), 1.2f))
                                      .Append(transform.DOMoveY(-8, 3f).SetEase(Ease.Linear))
                                      .AppendInterval(10f)
                                      .OnComplete(() =>
                                      {
                                          //Debug.Log("BOSS SKILL 종료");
                                          transform.position = new Vector2(0, 8);
                                          bossSpriteRenderer.DOColor(new Color(1, 1, 1, 1), 0);
                                          PageChange();
                                      });
        bossSkill_1 = _BossSkill_1;

        // Boss Skill_5 시퀀스
        _BossSkill_5 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMove(new Vector3(0, 8, 0), 1f))
                                      .Append(bossSpriteRenderer.DOColor(Color.clear, 1.5f))
                                      .OnComplete(() =>
                                      {
                                          bossCapsuleCollider2D.enabled = false;
                                          isRazer = true;
                                          transform.localScale = new Vector2(1, 1);
                                          transform.DOMove(Vector3.zero, 0);
                                          StartCoroutine(BossSkill_5());
                                      });
        bossSkill_5 = _BossSkill_5;
        #endregion

        StartCoroutine(BossStart()); // 최초 보스 등장 로직
        HealthManager(); // 보스 체력 값 초기화
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌 할때
        if (other.tag == "PBullet")
        {
            //Debug.Log("플레이어 탄과 충돌!");
            if (isRazer) return; // 레이저 스킬 사용 시 충돌 무시
            if (other.name != "BulletY") { GameManager.Instance.SkillPoint += 0.01f; }; // 기체의 스킬 포인트 획득
            bossAnimator.SetTrigger("isHit");
            Invoke("Attacked", 0.1f); // 이미지 복원
            other.gameObject.SetActive(false); // 총알 비활성화
            health -= other.GetComponent<Bullt>().power; // boss 체력 감소
            healthBar.SetHealth((int)health);

            if (health <= 0) // 체력이 0이되면
            {
                PageMoveStop(); // 보스 움직임 정지
                bossCapsuleCollider2D.enabled = false; // 충돌 제거
                isNextPage = true; // 페이지 변경 중 일때 충돌 제거
                currentPage++; // 페이지 증가
                if (currentPage > 3) {
                    BossEnd();
                    return;
                };
                HealthManager(); // 보스 체력 Bar 변경
                transform.DOMove(new Vector2(0, 8), 2) // 리스폰 장소로 복귀
                    .SetEase(Ease.Linear)
                    .OnComplete(() => // 복귀가 완료되면 크기 줄이기
                    {
                        transform.localScale = new Vector2(transform.localScale.x / 1.5f, transform.localScale.y / 1.5f);
                        saveSize = transform.localScale; // 사이즈 세이브
                    });
                Invoke("PageChange", 4); // 4초뒤에 페이지 변경 시작
            }
        }
    }

    #region PageChange() 페이지 변환 로직
    private void PageChange()
    {
        bossCapsuleCollider2D.enabled = true; // 충돌 켬
        isNextPage = false; // 페이지 변환 종료

        switch (currentPage)
        {
            case 1:
                //Debug.Log("페이지 1 시작");
                maxCountCheck = 0; // maxPatternCounts[]을 체크하기 위한 변수 초기화
                PageA(); // 총 발사
                PageAMove(); // 움직임 시작
                break;
            case 2:
                //Debug.Log("페이지 2 시작");
                maxCountCheck = 2; // maxPatternCounts[]을 체크하기 위한 변수 초기화
                Invoke("PageB", 3f); // 총 발사
                PageBMove(); // 움직임 시작
                break;
            case 3:
                //Debug.Log("페이지 3 시작");
                maxCountCheck = 4; // maxPatternCounts[]을 체크하기 위한 변수 초기화
                Invoke("PageC", 3f); // 총 발사
                PageCMove(); // 움직임 시작
                break;
        }
    }
    #endregion

    #region PageA() 첫번째 페이지 공격 로직
    private void PageA()
    {
        // 페이지 변경중 로직 정지를 위한 처리
        if (isNextPage)
        {
            //Debug.Log("페이지 변경중..");
            pattenCount = 0; // 패턴 횟수 초기화
            currentPattern = 1; // 현재 패턴종류 초기화
            return;
        }

        // pattenCount(총알 사용 횟수)가 maxPatternCounts(지정한 최대 발사 횟수) 비교
        // - 같다면 -1 (다음 패턴)
        // - 다르다면 1증가
        pattenCount = pattenCount == maxPatternCounts[maxCountCheck] ? -1 : pattenCount + 1;

        // 현재 패턴 
        switch (currentPattern)
        {
            // 일렬로 발사
            case 1:
                //Debug.Log("일렬로 발사!!");
                // 패턴별 발사 횟수 초과시
                if (pattenCount < 0)
                {
                    pattenCount = 0; // 패턴 횟수 초기화 
                    currentPattern++; // 다음 패턴++
                    maxCountCheck++; // 다음 패턴 횟수 체크할 변수++
                    Invoke("PageA", 1f); // 다음 패턴 쿨타임
                    return;
                }
                bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[9]);
                Shooting("EBL", 1); // 기본 탄 발사
                Invoke("PageA", 0.2f); // 0.2초마다 시작
                break;

            // 4개의 총탄 발사
            case 2:
                //Debug.Log("샷권 발사!!");
                if (pattenCount < 0)
                {
                    pattenCount = 0; // 패턴 횟수 초기화
                    currentPattern++; // 다음 패턴++
                    Invoke("PageA", 1f); // 다음 패턴 쿨타임
                    return;
                }

                // Boss의 총구 위치값으로 총 발사
                for (int index = 2; index < 6; index++)
                {
                    Shooting("EBM", index); // for문이므로 동시에 나가는 것 처럼 보임
                }
                bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[10]);
                Invoke("PageA", 0.2f); // 0.2초 마다 시작
                break;

            // 패턴 초기화
            default:
                //Debug.Log("패턴 초기화");
                maxCountCheck = 0; // 패턴 처음부터 시작
                currentPattern = 1; // 패턴 처음부터 시작
                PageA(); // 패턴 시작
                break;
        }
    }

    private void PageAMove()
    {
        //Debug.Log("PageAMove 호출");

        // 중복 시퀀스 사용을 방지하기 위한 처리
        //if (currentPage != 1)
        //{
        //    Debug.LogError("currentPageA Error is return");
        //    return;
        //}

        transform.DOMove(new Vector3(-2, 3, 0), 1.5f).SetEase(Ease.Linear) // 시작점을 -2로 시작, 균등한 움직임
            .OnComplete(() => // 도착시
            {
                SequenceUpdate(); // 시퀀스 초기화
                loopMoveSequence1.Restart(); // 좌우 루프 트윈 시작
            });

    }
    #endregion

    #region PageB() 두번째 페이지 공격 로직
    private void PageB()
    {
        // 페이지 변경중 로직 정지를 위한 처리
        if (isNextPage)
        {
            //Debug.Log("페이지 변경중..");
            pattenCount = 0; // 패턴 횟수 초기화
            currentPattern = 1; // 현재 패턴종류 초기화
            return;
        }

        // pattenCount(총알 사용 횟수)가 maxPatternCounts(지정한 최대 발사 횟수) 비교
        // - 같다면 -1 (다음 패턴)
        // - 다르다면 1증가
        pattenCount = pattenCount == maxPatternCounts[maxCountCheck] ? -1 : pattenCount + 1;

        // 현재 패턴 
        switch (currentPattern)
        {
            // 플레이어 방향으로 발사
            case 1:
                //Debug.Log("플레이어 방향으로 단발총 발사");

                // count를 다 발사했을 때 예외
                if (pattenCount < 0)
                {
                    pattenCount = 0; // 발사 횟수 초기화
                    currentPattern++; // 다음 패턴++
                    maxCountCheck++; // 다음 패턴 횟수 체크++
                    Invoke("PageB", 1f);
                    return;
                }
                bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[11]);
                BossSkill_4(); // 패턴 시작
                Invoke("PageB", 0.1f); // 0.1초 마다
                break;

            // Boss Skill_1
            case 2:
                //Debug.Log("스킬 시작!!");
                loopMoveSequence2.Pause(); // case 1의 움직임 정지
                BossSkill_1(); // 스킬 사용
                pattenCount = 0; // case 1로 복귀
                currentPattern = 1; // case 1로 복귀
                break;
        }
    }

    private void PageBMove()
    {
        //Debug.Log("PageBMove 호출");
        respawnBossSequence.Restart();
    }
    #endregion

    #region PageC() 세번째 페이지 공격 로직
    private void PageC()
    {
        // 페이지 변경중 로직 정지를 위한 처리
        if (isNextPage)
        {
            //Debug.Log("페이지 변경중..");
            pattenCount = 0; // 패턴 횟수 초기화
            currentPattern = 1; // 현재 패턴종류 초기화
            return;
        }

        // pattenCount(총알 사용 횟수)가 maxPatternCounts(지정한 최대 발사 횟수) 비교
        // - 같다면 -1 (다음 패턴)
        // - 다르다면 1증가
        pattenCount = pattenCount == maxPatternCounts[maxCountCheck] ? -1 : pattenCount + 1;

        // 현재 패턴 
        switch (currentPattern)
        {
            case 1:
                //Debug.Log("다발로 쏘기");
                // count다 발사 시 예외 처리
                if (pattenCount < 0)
                {
                    pattenCount = 0; // 발사 횟수 초기화
                    currentPattern++; // 다음 패턴++
                    maxCountCheck++; // 발사 횟수 체크++
                    Invoke("PageC", 2f); // 다음 패턴 쿨타임
                    return;
                }

                BossSkill_2(); // 스킬 사용
                Invoke("PageC", 0.5f); // 0.5초 마다 
                break;

            case 2:
                //Debug.Log("다발총 발사");

                if (pattenCount < 0)
                {
                    pattenCount = 0; // 발사 횟수 초기화
                    currentPattern++; // 다음 패턴++
                    maxCountCheck++; // 발사 횟수 체크++
                    Invoke("PageC", 2f); // 다음 패턴 쿨타임
                    return;
                }

                BossSkill_3(); // 스킬 사용
                Invoke("PageC", 0.05f); // 0.05초 마다
                break;

            case 3:
                //Debug.Log("레이저 발사 패턴");

                PageMoveStop(); // 움직임 리셋
                SequenceUpdate(); // 시퀸스 초기화
                bossSkill_5.Restart(); // 움직임 시작

                maxCountCheck = 4; // 횟수 체크 초기화
                currentPattern = 1; // 다음 패턴++
                pattenCount = 0; // 발사 횟수 초기화
                break;
        }
    }

    private void PageCMove()
    {
        //Debug.Log("PageBMove 호출");

        SequenceUpdate();
        respawnBossSequence.Restart();
    }
    #endregion

    #region BossSkill_1() BarSpawn Skill 로직
    private void BossSkill_1()
    {
        //Debug.Log("BOSSSKill_1() 실행");
        bossCapsuleCollider2D.enabled = false; // 충돌 제거
        SequenceUpdate(); // 시퀀스 초기화
        bossSkill_1.Restart(); // 시퀀스 시작
        StartCoroutine(BarSpawnerUpdate()); // 바 스폰 OnEnable
    }
    #endregion

    #region BossSkill_2() BulletSpawn Skill 로직
    private void BossSkill_2()
    {
        int[] dammeArry = RandomNum(); // 중복되지 않은 랜덤 값 배열
        bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[12]); // 소리 재생
        // 한번에 반복수 만큼 총알 생성
        for (int index = 0; index < 7; index++)
        {
            var bullets = ObjectManager.Instance.SpawnFromPool( // 오브젝트 생성
                                               "EBBM" // 총알 종류
                                             , new Vector2(screenWidths[dammeArry[index]], 5.3f) // 랜덤한 위치값 vector2
                                             , transform.rotation // 방향은 그대로
                                                           );
            bullets.transform.DOLocalMoveY(-4.7f, 2f).SetEase(Ease.Linear) // 생성 후 -4.7f 까지 움직임
                .OnComplete(() =>
                {
                    bullets.SetActive(false); // 완료 후 비활성 화
                });
        }
    }
    #endregion

    #region BossSkill_3() Machine Gun Skill 로직
    private void BossSkill_3()
    {
        int damme = Random.Range(1, 6); // 랜덤한 값
        var bullet = ObjectManager.Instance.SpawnFromPool( // 탄 생성
                                        "EBS" // 탄 총류
                                        , shootPoints[damme].position // 위치값 (랜덤한 보스 총구 위치)
                                        , transform.rotation); // 그대로
        bullet.transform.DOLocalMoveY(-4.7f, 3).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                bullet.SetActive(false); // 완료후 비활성화
            });
    }
    #endregion

    #region BossSkill_4() Follow Bullet Skill 로직
    private void BossSkill_4()
    {
        var bullet = ObjectManager.Instance.SpawnFromPool(
                                                            "EBS" // 탄 총류
                                                            , shootPoints[0].position // 위치값
                                                            , transform.rotation); // 회전

        bullet.transform.DOMove(playerPostion.position, 0.7f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                bullet.SetActive(false); // 완료후 비활성화
            });
    }
    #endregion

    #region BossSkill_5() Laser Skill 로직
    IEnumerator BossSkill_5()
    {
        int count = 0;
        yield return new WaitForSeconds(0.5f); // 시작전 대기시간

        while (true)
        {
            //Debug.Log("레이저 발사중!");
            if (count > 5) break;

            // line_1 세로 => player의 x축 추적

            bossLineRenderer_1.DOColor(resetColor, resetColor, 0); // 컬러 리셋

            bossLineRenderer_1.SetPosition(0, new Vector2(playerPostion.position.x, 5.5f)); // 라인 생성
            bossLineRenderer_1.SetPosition(1, new Vector2(playerPostion.position.x, -5.5f)); // 라인 생성
            edgeCollider_1.points = new Vector2[] { bossLineRenderer_1.GetPosition(0), bossLineRenderer_1.GetPosition(1) };
            yield return new WaitForSeconds(0.3f); // 라인 생성후
            bossLineRenderer_1.DOColor(startColor, endColor, 0.8f); // 라인 사라지는 모션
            edgeCollider_1.enabled = true; // 콜라이더 활성화
            bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[6]); // 소리 출력
            yield return new WaitForSeconds(0.3f);
            edgeCollider_1.enabled = false; // 콜라이더 비활성화

            bossLineRenderer_2.DOColor(resetColor, resetColor, 0); // 컬러 리셋

            bossLineRenderer_2.SetPosition(0, new Vector2(3, playerPostion.position.y)); ; // 라인 생성
            bossLineRenderer_2.SetPosition(1, new Vector2(-3, playerPostion.position.y)); // 라인 생성
            edgeCollider_2.points = new Vector2[] { bossLineRenderer_2.GetPosition(0), bossLineRenderer_2.GetPosition(1) };
            yield return new WaitForSeconds(0.3f); // 라인 생성후
            bossLineRenderer_2.DOColor(startColor, endColor, 0.8f); // 라인 사라지는 모션
            edgeCollider_2.enabled = true; // 콜라이더 활성화
            bossAudioSource.PlayOneShot(SoundManager.Instance.FXSounds[6]); // 소리 출력
            yield return new WaitForSeconds(0.3f);
            edgeCollider_2.enabled = false; // 콜라이더 비활성화

            count++;
        }
        //Debug.Log("레이저 발사 while 종료");

        isRazer = false;
        transform.localScale = saveSize;
        transform.DOMove(new Vector3(0, 8, 0), 0); // 위치 리셋
        bossSpriteRenderer.DOColor(Color.white, 0.2f) // 색 리셋
            .OnComplete(() =>
            {
                PageChange(); // 페이지 체인지
            });
    }
    #endregion

    #region Shooting() 총 발사 로직
    // 탄 종류, 위치값을 받음
    private void Shooting(string bulletName, int postion)
    {
        ObjectManager.Instance.SpawnFromPool(bulletName, shootPoints[postion].position, transform.rotation);
    }
    #endregion

    #region PageMoveStop() 보스 움직임 시퀀스 정지
    private void PageMoveStop()
    {
        //Debug.Log("페이지 무브 스탑");
        loopMoveSequence1.Pause();
        loopMoveSequence2.Pause();
        loopMoveSequence3.Pause();
        respawnBossSequence.Pause();
    }
    #endregion

    #region SequenceUpdate() 시퀀스 초기화 로직
    private void SequenceUpdate()
    {
        //Debug.Log("시퀀스 초기화!");
        loopMoveSequence1 = _LoopMoveSequence_1;
        loopMoveSequence2 = _LoopMoveSequence_2;
        loopMoveSequence3 = _LoopMoveSequence_3;
        bossSkill_1 = _BossSkill_1;
        bossSkill_5 = _BossSkill_5;
        respawnBossSequence = _RespawnBossSequence;
    }
    #endregion

    #region BarSpawnerUpdate() Bar 장애물 생성로직
    private IEnumerator BarSpawnerUpdate()
    {
        barSpawner.enabled = true;
        yield return new WaitForSeconds(11f);
        barSpawner.enabled = false;
    }
    #endregion

    #region ScreenMath() 화면 비율에 따른 위치 계산 로직
    private void ScreenMath()
    {
        mainCamWidth = Screen.width;
        //Debug.Log($"계산전 mainCam : {mainCamWidth}"); //540
        mainCamWidth = mainCamWidth / 100f;
        //Debug.Log($"계산후 mainCam : {mainCamWidth}"); // 5.4
        float num = mainCamWidth / 8f;
        //Debug.Log($"num  : {num}"); // 0.675
        float halfNum = mainCamWidth / 2f;
        //Debug.Log($"halfNum의 값 : {halfNum}"); // 2.7
        float saveHalfNum = halfNum;

        screenWidths[0] = saveHalfNum;

        for (int index = 1; index < 9; index++)
        {
            float temp = 0;
            temp = halfNum - num; // 2.7 - 0.675 = 2.025
            screenWidths[index] = temp;
            halfNum = temp;
            //Debug.Log($"screenWidths 배열의 {index} 값 : {screenWidths[index]}");
        }
    }
    #endregion

    #region RandomNum() 중복되지않은 난수 발생기 로직
    private int[] RandomNum()
    {
        int ranLength = 9;

        int[] ranArr = Enumerable.Range(0, ranLength).ToArray();

        for (int i = 0; i < ranLength; ++i)
        {
            int ranIdx = Random.Range(i, ranLength);

            int tmp = ranArr[ranIdx];

            ranArr[ranIdx] = ranArr[i];

            ranArr[i] = tmp;
        }

        return ranArr;
    }
    #endregion

    #region HealthManager() 보스 체력 관리
    private void HealthManager()
    {
        switch (currentPage)
        {
            case 1:
                health = 5000; // 1 page 체력
                break;
            case 2:
                health = 4000; // 2 page 체력
                break;
            case 3:
                health = 7000; // 3 page 체력
                break;
        }

        healthBar.SetMaxHealth((int)health); // 보스 체력바 최대 값 초기화

    }
    #endregion

    #region BossStart() 보스 생성 로직
    IEnumerator BossStart()
    {
        bossAudioSource.loop = true; // loop 상태로 만듬
        bossAudioSource.clip = SoundManager.Instance.FXSounds[7]; // 사운드 클립 교체 (경고 벨)
        bossAudioSource.Play(); // 소리 재생
        bossAudioSource.DOFade(0, 4f) // 4초안에 소리 꺼짐
            .OnComplete(() =>
            {
                bossAudioSource.Stop(); // 소리를 멈춤
                bossAudioSource.volume = 0.3f; // 볼륨 초기화
            });
        yield return new WaitForSeconds(2f); // 대기 시간
        transform.DOMoveY(3, 3).SetEase(Ease.Linear); // 초기 위치값 시작
        Invoke("PageChange", 3.1f); // 현재 페이지에 맞는 공격 패턴 시작
    }
    #endregion

    #region BossEnd() 보스 처지 로직
    private void BossEnd()
    {
        Debug.Log("보스 처치");
        SoundManager.Instance.GetComponent<AudioSource>().DOFade(0, 3f);
        GameManager.Instance.isWin = true;
        Sequence damme = DOTween.Sequence()
        .Append(transform.DOLocalMove(new Vector2(2, -2), 4).SetRelative().SetEase(Ease.Linear))
        .Join(transform.DOScale(0, 4).SetEase(Ease.Linear))
        .Append(transform.DOScale(0.5f,0))
        .OnComplete(() => {
            SoundManager.Instance.GetComponent<AudioSource>().clip = null;
            SoundManager.Instance.GetComponent<AudioSource>().volume = 0.6f;
            ObjectManager.Instance.SpawnFromPool("BossDestroyFX", transform.position, transform.rotation);
            gameObject.SetActive(false);
        }); 
    }
    #endregion
}

