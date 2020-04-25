using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    public AudioClip[] PlayerSounds; // 플레이어 사운드
    public AudioClip[] EnemySounds; // 적 기체 사운드
    public AudioClip[] FXSounds; // 이팩트 사운드

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator BossSound()
    {
        Debug.Log("보스 사운드 코루틴 실행");
        Tween myTween = audioSource.DOFade(0, 2f);
        yield return myTween.WaitForCompletion();
        Debug.Log("사운드 트윈 종료");
        audioSource.Stop(); // 정지
        audioSource.clip = FXSounds[8]; // background sound 교체
        audioSource.volume = 0.6f;
        yield return new WaitForSeconds(3f); // 1초 대기후
        audioSource.Play(); // 플레이
    }
}
