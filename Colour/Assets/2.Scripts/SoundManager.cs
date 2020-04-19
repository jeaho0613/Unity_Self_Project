using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    public AudioClip bullet; // 총알 소리
    public AudioClip colorChange; // 색 변경 소리
    public AudioClip playerDie; // 플레이어 죽는 소리
    public AudioClip endGame; // 게임 종료 소리

    private void Awake()
    {
        Instance = this;
    }
}
