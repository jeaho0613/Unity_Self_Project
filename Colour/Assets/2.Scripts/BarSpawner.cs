using System.Collections;
using UnityEngine;

// Bar 장애물 스폰 스크립트
public class BarSpawner : MonoBehaviour
{
    private float delayTime; // 스폰 딜레이
    private string[] colos = { "BarR", "BarG", "BarB"}; // 스폰될 바(오브젝트)
   
    private void Start()
    {
        StartCoroutine(CreateBar()); // 코루틴 시작
    }

    // 바 생성 로직
    IEnumerator CreateBar()
    {
        // 무한 루프
        while (true)
        {
            int num = Random.Range(0, 3); // 랜덤한 색상
            delayTime = Random.Range(1, 5); // 랜덤한 생성 시간
            yield return new WaitForSeconds(delayTime); // 대기 시간
            ObjectManager.Instance.SpawnFromPool(colos[num], transform.position, Quaternion.identity); // 오브젝트 풀에서 생성
        }
    }
}
