using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Spawn 구조체
    public class Spawn
    {
        public float delay; // 생성시간
        public string type; // 생성타입
        public int point; // 생성위치
    }

    public List<Spawn> spawnList; // Spawn구조체 리스트

    private void Awake()
    {
        Instance = this;
        spawnList = new List<Spawn>();
        ReadSpawnFile();
    }

    #region Text파일 읽기
    void ReadSpawnFile()
    {
        spawnList.Clear(); // 초기화

        // Resources폴더의 stage 0 텍스트 읽기
        TextAsset textFile = Resources.Load("stage 0") as TextAsset;
        // StringReader로 파일 열기
        StringReader stringReader = new StringReader(textFile.text);

        // 마지막 줄을 읽을 때의 예외처리
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            //Debug.Log(line); //읽은값 출력

            // 마지막줄 즉 null 값이면 정지
            if (line == null)
            {
                break;
            }

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }
        stringReader.Close();
    }
    #endregion
}
