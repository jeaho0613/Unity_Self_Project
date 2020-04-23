using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Boss : MonoBehaviour
{
    public bool isNextPage = false; // 페이지 변환 체크
    public float health; // 체력
    public int maxCountCheck; // 최대 발사 체크
    public int pattenCount; // 발사 횟수
    public int currentPattern; // 현재 패턴
    public int currentPage; // 현재 보스 페이지
    public Transform[] shootPoints; // 총 발사 포인트
    public int[] maxPatternCounts; // 각 패턴발사 수

    //private PathType pathType = PathType.CatmullRom; // path 타입 (직선 경로)
    private float saveHealth; // 체력 저장 변수

    private CapsuleCollider2D bossCapsuleCollider2D; // Boss 콜라이더
    private SpriteRenderer bossSpriteRenderer; // Boss spriteRenderer
    [SerializeField]
    private Transform playerPostion; // 플레이어 위치값

    // Move 사용 시퀀스
    private Sequence mySequence1; // page 1 
    private Sequence mySequence2; // page 2
    private Sequence mySequence3; // page 3
    private Sequence bossSkill_1; // skill 1

    private Sequence _MySequence1; // page 1 
    private Sequence _MySequence2; // page 2
    private Sequence _MySequence3; // page 3
    private Sequence _BossSkill_1; // skill 1

    private void Awake()
    {
        bossCapsuleCollider2D = GetComponent<CapsuleCollider2D>(); // 초기화
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        playerPostion = FindObjectOfType<PlayerControll>().transform;

        currentPage = 1; // 페이지 초기화
        currentPattern = 1; // 페턴 초기화
        saveHealth = health; // 체력 저장 변수 초기화
        isNextPage = true; // 처음 소환시 탄 발사 정지
    }

    private void Start()
    {
        transform.DOMoveY(3, 3).SetEase(Ease.Linear); // 초기 위치값 시작
        Invoke("PageChange", 3.1f); // 현재 페이지에 맞는 공격 패턴 시작

        #region DOTween 초기화

        // move1 시퀀스
        _MySequence1 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2, 3f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2, 3f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        mySequence1 = _MySequence1;

        // move2 시퀀스
        _MySequence2 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        mySequence2 = _MySequence2;

        // move3 시퀀스
        _MySequence3 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
                                      .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
                                      .SetLoops(-1, LoopType.Yoyo);
        mySequence3 = _MySequence3;

        // Boss skill 시퀀스
        _BossSkill_1 = DOTween.Sequence().Pause()
                                      .SetAutoKill(false)
                                      .Append(transform.DOMove(new Vector3(0, 8, 0), 1f))
                                      .Append(bossSpriteRenderer.DOColor(new Color(0, 0, 0, 0.5f), 1.5f))
                                      .Append(transform.DOMoveY(-8, 4f).SetEase(Ease.Linear))
                                      .AppendInterval(10f)
                                      .OnComplete(() =>
                                      {
                                          Debug.Log("BOSS SKILL 종료");
                                          transform.position = new Vector2(0, 8);
                                          bossSpriteRenderer.DOColor(new Color(1, 1, 1, 1), 0);
                                          PageChange();
                                      });

        bossSkill_1 = _BossSkill_1;
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 총알과 충돌 할때
        if (other.tag == "PBullet")
        {
            other.gameObject.SetActive(false); // 총알 비활성화
            health -= other.GetComponent<Bullt>().power; // boss 체력 감소
            if (health <= 0) // 체력이 0이되면
            {
                PageMoveStop(); // 보스 움직임 정지
                bossCapsuleCollider2D.enabled = false; // 충돌 제거
                isNextPage = true;
                currentPage++; // 페이지 증가
                health = saveHealth; // 체력 복귀
                transform.DOMove(new Vector2(0, 8), 2) // 리스폰 장소로 복귀
                    .SetEase(Ease.Linear)
                    .OnComplete(() => // 복귀가 완료되면 크기 줄이기
                    {
                        transform.DOScale(new Vector2
                            (
                            transform.localScale.x / 1.5f, // 크기 줄이기
                            transform.localScale.y / 1.5f), // 크기 줄이기
                            0 // 곧 바로 변경 
                            );
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
                Debug.Log("페이지 1 시작");
                maxCountCheck = 0;
                PageA();
                PageAMove();
                break;
            case 2:
                Debug.Log("페이지 2 시작");
                maxCountCheck = 2;
                Invoke("PageB", 3f); // 총 발사
                PageBMove();
                break;
            case 3:
                Debug.Log("페이지 3 시작");
                maxCountCheck = 4;
                Invoke("PageC", 3f); // 총 발사
                PageCMove();
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
            Debug.Log("페이지 변경중..");
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
                Debug.Log("일렬로 발사!!");
                if (pattenCount < 0)
                {
                    pattenCount = 0;
                    currentPattern++;
                    maxCountCheck++;
                    Invoke("PageA", 1f);
                    return;
                }

                Shooting("EBL", 1);
                Invoke("PageA", 0.2f);
                break;

            // 4개의 총탄 발사
            case 2:
                Debug.Log("샷권 발사!!");
                if (pattenCount < 0)
                {
                    pattenCount = 0;
                    currentPattern++;
                    Invoke("PageA", 1f);
                    return;
                }

                for (int index = 2; index < 6; index++)
                {
                    Shooting("EBM", index);
                }
                Invoke("PageA", 0.2f);
                break;

            // 패턴 초기화
            default:
                Debug.Log("패턴 초기화");
                maxCountCheck--;
                currentPattern = 1;
                PageA();
                break;
        }
    }

    private void PageAMove()
    {
        Debug.Log("PageAMove 호출");
        if (currentPage != 1)
        {
            Debug.LogError("currentPageA Error is return");
            return;
        }

        transform.DOMove(new Vector3(-2, 3, 0), 1.5f).SetEase(Ease.Linear) // 시작점을 -2로 시작, 균등한 움직임
            .OnComplete(() => // 도착시
            {
                SequenceUpdate(); // 시퀀스 초기화
                mySequence1.Restart(); // 좌우 루프 트윈 시작
            });

    }
    #endregion

    #region PageB() 두번째 페이지 공격 로직
    private void PageB()
    {
        // 페이지 변경중 로직 정지를 위한 처리
        if (isNextPage)
        {
            Debug.Log("페이지 변경중..");
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
                Debug.Log("플레이어 방향으로 단발총 발사");
                if (pattenCount < 0)
                {
                    pattenCount = 0;
                    currentPattern++;
                    maxCountCheck++;
                    Invoke("PageB", 1f);
                    return;
                }

                var bullet = ObjectManager.Instance.SpawnFromPool("EBS", shootPoints[0].position, transform.rotation);
                bullet.transform.DOMove(playerPostion.position, 0.7f).SetEase(Ease.Linear)
                    .OnComplete(() => { bullet.SetActive(false); });
                Invoke("PageB", 0.2f);
                break;

            // 4개의 총탄 발사
            case 2:
                Debug.Log("스킬 시작!!");
                mySequence2.Pause();
                BossSkill_1();
                pattenCount = 0;
                currentPattern = 1;

                break;

            // 패턴 초기화
            default:
                Debug.Log("패턴 초기화");
                maxCountCheck--;
                currentPattern = 1;
                PageB();
                break;
        }
    }

    private void PageBMove()
    {
        Debug.Log("PageBMove 호출");
        if (currentPage != 2)
        {
            Debug.LogError("currentPageB Error is return");
            return;
        }

        Sequence damme = DOTween.Sequence()
            .Append(transform.DOMove(new Vector3(0, 3, 0), 3f)).SetEase(Ease.Linear) // 시작점 조정
            .Append(transform.DOMoveX(-2, 1.5f)).SetEase(Ease.Linear)
            .OnComplete(() => // 도착시
            {
                SequenceUpdate(); // 시퀀스 초기화
                mySequence2.Restart(); // 좌우 루프 트윈 시작
            });
    }
    #endregion

    #region PageC() 세번째 페이지 공격 로직
    private void PageC()
    {
        // 페이지 변경중 로직 정지를 위한 처리
        if (isNextPage)
        {
            Debug.Log("페이지 변경중..");
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
                Debug.Log("일렬로 발사!!");
                Shooting("EBL", 1);
                if (pattenCount < 0)
                {
                    pattenCount = 0;
                    currentPattern++;
                    maxCountCheck++;
                    Invoke("PageC", 1f);
                    return;
                }
                Invoke("PageC", 0.2f);
                break;

            // 4개의 총탄 발사
            case 2:
                Debug.Log("샷권 발사!!");
                for (int index = 2; index < 6; index++)
                {
                    Shooting("EBM", index);
                }
                if (pattenCount < 0)
                {
                    pattenCount = 0;
                    currentPattern++;
                    Invoke("PageC", 1f);
                    return;
                }
                Invoke("PageC", 0.2f);
                break;

            // 패턴 초기화
            default:
                Debug.Log("패턴 초기화");
                maxCountCheck--;
                currentPattern = 1;
                PageC();
                break;
        }
    }

    private void PageCMove()
    {
        Debug.Log("PageBMove 호출");
        if (currentPage != 3)
        {
            Debug.LogError("currentPageC Error is return");
            return;
        }
        SequenceUpdate(); // 시퀀스 초기화
        mySequence3.Restart();
    }
    #endregion

    #region BossSkill_1() 보스 스킬 로직

    private void BossSkill_1()
    {
        Debug.Log("BOSSSKill_1() 실행");
        bossCapsuleCollider2D.enabled = false;
        SequenceUpdate(); // 시퀀스 초기화
        bossSkill_1.Restart();
    }

    #endregion

    #region Shooting() 총 발사 로직
    private void Shooting(string bulletName, int postion)
    {
        ObjectManager.Instance.SpawnFromPool(bulletName, shootPoints[postion].position, transform.rotation);
    }
    #endregion

    #region PageMoveStop() 보스 움직임 시퀀스 정지
    private void PageMoveStop()
    {
        mySequence1.Pause();
        mySequence2.Pause();
        mySequence3.Pause();
    }
    #endregion

    private void SequenceUpdate()
    {
        Debug.Log("시퀀스 초기화!");
        bossSkill_1 = _BossSkill_1;
        mySequence1 = _MySequence1;
        mySequence2 = _MySequence2;
        mySequence3 = _MySequence3;
    }

}
