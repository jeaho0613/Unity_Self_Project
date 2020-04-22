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
    public Transform[] shootPoints; // 총 발사 포지션
    public int[] maxPatternCounts; // 각 패턴발사 수

    private PathType pathType = PathType.CatmullRom; // path 타입 (직선 경로)
    private float saveHealth; // 체력 저장 변수
    private CapsuleCollider2D capsuleCollider2D; // BOSS 콜라이더
    //private IEnumerator pageAMove;
    //private IEnumerator pageBMove;
    //private IEnumerator pageCMove;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>(); // 초기화
        currentPage = 1; // 페이지 초기화
        currentPattern = 1; // 페턴 초기화
        saveHealth = health; // 체력 저장 변수 초기화

        //pageAMove = PageAMove(); // 코루틴 초기화
        //pageBMove = PageBMove(); // 코루틴 초기화
        //pageCMove = PageCMove(); // 코루틴 초기화
    }

    private void Start()
    {
        transform.DOMoveY(3, 3).SetEase(Ease.Linear); // 초기 위치값 시작
        Invoke("PageChange", 3.1f); // 현재 페이지에 맞는 공격 패턴 시작
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
                capsuleCollider2D.enabled = false; // 충돌 제거
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
                Invoke("PageChange", 3); // 3초뒤에 페이지 변경 시작
            }
        }
    }

    #region PageChange() 페이지 변환 로직
    private void PageChange()
    {
        capsuleCollider2D.enabled = true; // 충돌 켬
        isNextPage = false; // 페이지 변환 종료
        switch (currentPage)
        {
            case 1:
                Debug.Log("페이지 1 시작");
                PageA();
                PageAMove();
                break;
            case 2:
                Debug.Log("페이지 2 시작");
                PageB();
                PageBMove();
                break;
            case 3:
                Debug.Log("페이지 3 시작");
                PageC();
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
            currentPattern = 0; // 현재 패턴종류 초기화
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
                    Invoke("PageA", 1f);
                    return;
                }
                Invoke("PageA", 0.2f);
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
                    Invoke("PageA", 1f);
                    return;
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
            return;

        Sequence mySequence1 = DOTween.Sequence()
            .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
            .OnComplete(() => { PageAMove(); });
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
            currentPattern = 0; // 현재 패턴종류 초기화
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
                    Invoke("PageB", 1f);
                    return;
                }
                Invoke("PageB", 0.2f);
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
                    Invoke("PageB", 1f);
                    return;
                }
                Invoke("PageB", 0.2f);
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
        Debug.Log("PageAMove 호출");
        if (currentPage != 2)
            return;

        Sequence mySequence2 = DOTween.Sequence()
            .Append(transform.DOMoveY(3, 3).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
            .OnComplete(() => { PageBMove(); });
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
            currentPattern = 0; // 현재 패턴종류 초기화
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
        Debug.Log("PageAMove 호출");
        if (currentPage != 3)
            return;

        Sequence mySequence3 = DOTween.Sequence()
            .Append(transform.DOMoveY(3, 3).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(-2, 1.5f).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(2, 1.5f).SetEase(Ease.Linear))
            .OnComplete(() => { PageCMove(); });
    }
    #endregion

    #region Shooting() 총 발사 로직
    private void Shooting(string bulletName, int postion)
    {
        ObjectManager.Instance.SpawnFromPool(bulletName, shootPoints[postion].position, transform.rotation);
    }
    #endregion
}
