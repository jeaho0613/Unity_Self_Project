using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip bullet;
    public AudioClip colorChange;
    public AudioClip playerDie;
    public AudioClip endGame;

    private void Awake()
    {
        Instance = this;
    }
}
