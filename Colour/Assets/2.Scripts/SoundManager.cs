using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    public AudioClip[] PlayerSounds; // 플레이어 사운드
    public AudioClip[] EnemySounds; // 적 기체 사운드
    public AudioClip[] FXSounds; // 이팩트 사운드

    private void Awake()
    {
        Instance = this;
    }
}
