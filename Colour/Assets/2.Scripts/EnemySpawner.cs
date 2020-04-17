using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] points; // 생성 위치
    public GameObject playerObject; // playerObject

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
        nextSpawnTime += Time.deltaTime; // 경과 시간

        // 마지막줄 처리
        if (enemySpawnList.Count <= index)
        {
            // 인덱스를 리셋
            index = 0;
        }

        // 경과시간 > 딜레이 
        if (nextSpawnTime > enemySpawnList[index].delay)
        {
            //Debug.Log($"index : {index}"); // 현재 인덱스 
            nextSpawnTime = 0; // 생성 주기 초기화
            EnemySpawn(index); // 적 기체 생성
        }
    }

    // 적 기체 생성 로직
    private void EnemySpawn(int index)
    {
        //Debug.Log($"{index}번째 생성함."); // 현재 인덱스

        switch (enemySpawnList[index].point)
        {
            // 0,1,2 포인트 생성 직선주행
            case 0:
            case 1:
            case 2:
                playerPoint = playerObject.transform.position;
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type
                                                    , points[enemySpawnList[index].point].position
                                                    , Quaternion.identity);
                
                enemyObject.transform.DOMoveY(-5.5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.Linear);

                //Debug.Log("0,1,2 포인트 생성 입니다.");
                break;
            // 3,5 포인트 생성 플레이어 방향으로
            case 3:
            case 5:
                playerPoint = playerObject.transform.position;
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type
                                                     , points[enemySpawnList[index].point].position
                                                     , Quaternion.identity);
                enemyObject.transform.DOMoveX(playerPoint.x, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.OutQuad);
                enemyObject.transform.DOMoveY(-5.5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.InQuad);


                //Debug.Log("3,5 포인트 생성 입니다.");
                break;
            // 4,6 포인트 생성 플레이어 방향으로 
            case 4:
            case 6:
                playerPoint = playerObject.transform.position;
                enemyObject = ObjectManager.Instance.SpawnFromPool(enemySpawnList[index].type
                                                     , points[enemySpawnList[index].point].position
                                                     , Quaternion.identity);
                enemyObject.transform.DOMoveX(playerPoint.x, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.OutQuad);
                enemyObject.transform.DOMoveY(-5.5f, enemyObject.GetComponent<Enemy>().endSpeed).SetEase(Ease.InQuad);

                //Debug.Log("4,6 포인트 생성 입니다.");
                break;
        }

        this.index++;
    }
}
