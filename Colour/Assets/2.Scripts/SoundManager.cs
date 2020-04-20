using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    public AudioClip[] PlayerSounds;
    public AudioClip[] EnemySounds;
    public AudioClip[] FXSounds;

    private void Awake()
    {
        Instance = this;
    }
}
