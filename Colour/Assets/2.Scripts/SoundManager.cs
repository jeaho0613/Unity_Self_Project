using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    public AudioClip[] PlayerSounds; // 플레이어 사운드
    public AudioClip[] EnemySounds; // 적 기체 사운드
    public AudioClip[] FXSounds; // 이팩트 사운드

    private AudioSource audioSource; // 오디오 소스

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>(); // 초기화
    }

    public IEnumerator BossSound()
    {
        //Debug.Log("보스 사운드 코루틴 실행");
        Tween myTween = audioSource.DOFade(0, 2f); // 소리 페이드 아웃
        yield return myTween.WaitForCompletion(); // 트윈이 완료 된 후
        //Debug.Log("사운드 트윈 종료");
        audioSource.Stop(); // 정지
        audioSource.clip = FXSounds[8]; // background sound 교체
        audioSource.volume = 0.6f; // 볼륨 초기화
        yield return new WaitForSeconds(3f); // 1초 대기후
        audioSource.Play(); // 플레이
    }
}
