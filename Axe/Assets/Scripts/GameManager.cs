using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    // Star 획득시 얻는 포인트
    // - 배경색이 밝아지는 값을 나타냄
    public int Score = 0;
    public Camera maincam;

    private void Update()
    {
        if (Score >= 1)
            maincam.backgroundColor = new Color(255, 255, 255);
    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
