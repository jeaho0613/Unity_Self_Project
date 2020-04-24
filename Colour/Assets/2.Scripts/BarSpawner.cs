using System.Collections;
using UnityEngine;

// Bar 장애물 스폰 스크립트
public class BarSpawner : MonoBehaviour
{
    private float delayTime; // 스폰 딜레이
    private string[] colos = { "BarR", "BarG", "BarB" }; // 스폰될 바(오브젝트)
    private Vector3 spawnPostion = new Vector3(0,6,0); // 생성 위치

    private void OnEnable()
    {
        // 바 생성 로직
        StartCoroutine("CreateBar"); // 코루틴 시작
    }

    private void OnDisable()
    {
        StopCoroutine("CreateBar"); // 코루틴 정지
    }

    IEnumerator CreateBar()
    {
        yield return new WaitForSeconds(4.5f); // 대기시간

        while (true)
        {
            int num = Random.Range(0, 3); // 랜덤한 색상
            delayTime = Random.Range(0.3f, 0.5f); // 랜덤한 생성 시간
            yield return new WaitForSeconds(delayTime); // 대기 시간
            ObjectManager.Instance.SpawnFromPool(colos[num], spawnPostion, Quaternion.identity); // 오브젝트 풀에서 생성
        }
    }

}
