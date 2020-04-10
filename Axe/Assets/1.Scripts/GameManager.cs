using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int LoadPoint; // stage 역할, LoadPoint 지점

    public Camera maincam; // 메인 카메라
    public GameObject[] starActive; // star획득 Active

    private static GameManager instance = null;

    // GameManager 싱글톤
    // - 초기화
    private void Awake()
    {
        // 만약 instace가 null 값이면
        if (null == instance)
        {
            // instace에 GameManager 데이터 넣어줌
            instance = this;
            // 씬이 변경되거나 오브젝트가 삭제되는걸 방지
            DontDestroyOnLoad(this.gameObject);
        }
        // 만약 중복되는 GameManager가 있을 경우
        // - 삭제함 (기존 GameManager를 남겨둠)
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        // get (가져올때) 
        get
        {
            // instance가 null이면
            if (null == instance)
            {
                // null을 돌려줌
                // - Awake에서 초기화 해줌
                return null;
            }
            // null값이 아니면 instance return
            return instance;
        }
    }

    private void Start()
    {
        // 최초 게임 시작시 LoadPoint에 따라 star 이미지 활성화
        for (int index = 0; index < LoadPoint; index++)
        {
            starActive[index].SetActive(true); // 별 이미지 활성화
        }
    }
}
