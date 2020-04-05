using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] changeColors; // chageColors 오브젝트
    public GameObject[] stages; // stages 오브젝트
    public Transform[] loadPoint; // 플레이어 로드될 위치
    public GameObject player; // player postion 정보를 위한 변수

    // Stage 비,활성화 동작
    public void StageActive()
    {
        // GameManager의 LoadPoint(스테이지) 값을 가져와서
        int loadPoint = GameManager.Instance.LoadPoint;
        // 그 값에 따라서 스테이지를 로드함
        switch (loadPoint)
        {
            case 0:
                stages[0].SetActive(true); // 스테이지 0 활성화 (첫 시작)
                break;
            case 1:
                stages[0].SetActive(false); // 스테이지 0 비활성화
                stages[1].SetActive(true); // 스테이지 1 활성화
                break;
            case 2:
                stages[1].SetActive(false); // 스테이지 1 비활성화
                stages[2].SetActive(true); // 스테이지 2 활성화
                break;
            case 3:
                stages[2].SetActive(false); // 스테이지 2 비활성화
                stages[3].SetActive(true); // 스테이지 3 활성화
                break;
            case 4:
                stages[3].SetActive(false); // 스테이지 3 비활성화
                stages[4].SetActive(true); // 스테이지 4 활성화
                break;
        }
    }

    // Chagecolor 오브젝트의 동작
    public void ChangeColorActive()
    {
        int count = changeColors[0].transform.childCount; // 개수 파악
        //Debug.Log(count);
        int stage = GameManager.Instance.LoadPoint; // LoadPoint를 받아와서
        for (int index = 0; index < count; index++)
        {
            // chagecolor의 자식들을 활성화
            changeColors[stage].transform.GetChild(index).gameObject.SetActive(true);
        }
    }
    // Player의 위치 동작
    public void SetPostion()
    {
        // loadPoint(스테이지에 따라 위치값을 변환
        player.transform.position = loadPoint[GameManager.Instance.LoadPoint].position;
    }

}
