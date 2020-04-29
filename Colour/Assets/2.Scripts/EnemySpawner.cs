using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] points; // 생성 위치
    public GameObject playerObject; // playerObject
    public Vector2[] wayPoints = new Vector2[4];
    public PathType pathType = PathType.Linear;

    private Vector3 playerPoint; // player 위치값
    private GameObject enemyObject; // 적 기체
    private int index; // enemySpawnList의 index
    private float nextSpawnTime; // 다음 생성 주기
    private List<GameManager.Spawn> enemySpawnList; // Stage.txt로 받은 데이터

    private void Awake()
    {
        index = 0; // 초기화
        enemySpawnList = GameManager.Instance.spawnList; // 초기화
        //Debug.Log("enemySpanwList count : " + enemySpawnList.Count); // 리스트 총 길이
    }

    private void Update()
    {
        if (GameManager.Instance.isBoss) return; // 보스전이면 동작 X

        nextSpawnTime += Time.deltaTime; // 경과 시간

        // 마지막줄 처리
        if (enemySpawnList.Count <= index)
        {
            //Debug.Log("마지막 적 생성 종료");
            StartCoroutine(SoundManager.Instance.BossSound());
            GameManager.Instance.isBoss = true; // 보스전 돌입
            //gameObject.SetActive(false); // 몹 스포너 제거
        }

        // 경과시간 > 딜레이 
        else if (nextSpawnTime > enemySpawnList[index].delay)
        {
            //Debug.Log("적 생성"); // 현재 인덱스 
            nextSpawnTime = 0; // 생성 주기 초기화
            EnemySpawn(index); // 적 기체 생성
        }
    }

    #region EnemySpawn() 적 기체 생성 로직
    private void EnemySpawn(int index)
    {
        //Debug.Log($"{index}번째 생성함."); // 현재 인덱스

        switch (enemySpawnList[index].point)
        {
            // BOSS 생성
            case 0:
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                    , points[enemySpawnList[index].point].position // 생성 포인트
                                                    , Quaternion.identity); // 회전값
                break;
            // Red Large 기체 생성
            case 1:
            case 2:
                
            case 3:
                // 오브젝트 생성
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                    , points[enemySpawnList[index].point].position // 생성 포인트
                                                    , Quaternion.identity); // 회전값

                // 움직임 (직선)
                enemyObject.transform.DOMoveY(-5.5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);
                break;

            // Red M 기체 생성
            case 4:
                // 오브젝트 생성
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                     , points[enemySpawnList[index].point].position // 생성 포인트
                                                     , Quaternion.Euler(0, 0, 90)); // 회전값

                // 움직임 (대각선)
                enemyObject.transform.DOMoveX(5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);
                enemyObject.transform.DOMoveY(0f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);
                break;

            // Red M 기체 생성
            case 5:
                // 오브젝트 생성
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                     , points[enemySpawnList[index].point].position // 생성 포인트
                                                     , Quaternion.Euler(0, 0, -90)); // 회전값

                // 움직임 (대각선)
                enemyObject.transform.DOMoveX(-5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);
                enemyObject.transform.DOMoveY(0f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);
                break;

            // Red S 기체 생성
            case 6:
            case 7:
                // 오브젝트 생성
                playerPoint = playerObject.transform.position; // 현재 player 위치값
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                     , points[enemySpawnList[index].point].position // 생성 포인트
                                                     , Quaternion.identity); // 회전값

                // 움직임 (플레이어.X값으로 진행)
                enemyObject.transform.DOMoveX(playerPoint.x, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.OutQuad);
                enemyObject.transform.DOMoveY(-5.5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.InQuad);
                break;

            // Enemy Blue 기체 생성
            case 8:
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                    , points[enemySpawnList[index].point].position // 생성 포인트
                                                    , Quaternion.identity); // 회전값

                enemyObject.transform.DOMoveX(4, 8).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
                break;
            // Enemy Blue 기체 생성
            case 9:
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                    , points[enemySpawnList[index].point].position // 생성 포인트
                                                    , Quaternion.identity); // 회전값

                enemyObject.transform.DOMoveX(-4, 8).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                break;
            case 10:
                   enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type // 생성 오브젝트 타입
                                                    , points[enemySpawnList[index].point].position // 생성 포인트
                                                    , Quaternion.identity); // 회전값

                Sequence damme = DOTween.Sequence()
                .Append(enemyObject.transform.DOMove(new Vector2(2.5f, 3), 1)) // 이동할 위치 포인트
                .Append(enemyObject.transform.DOMove(new Vector2(-2.5f, 1), 1)) // 이동할 위치 포인트
                .Append(enemyObject.transform.DOMove(new Vector2(2.5f, -1), 1)) // 이동할 위치 포인트
                .Append(enemyObject.transform.DOMove(new Vector2(-2.5f, -3), 1)) // 이동할 위치 포인트
                .SetEase(Ease.Linear) // 평균적인 속도로
                .SetLoops(-1,LoopType.Yoyo); // 무한 루프
                break;
        }

        this.index++; // 다음 생성을 위한 index값 증가
        //Debug.Log(index);
    }
    #endregion
}
