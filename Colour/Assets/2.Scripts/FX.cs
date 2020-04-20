using UnityEngine;
using System.Collections;

public class FX : MonoBehaviour, IPooledObject
{
    private Animator animator; // 애니메이션
    private AudioSource audioSource; // 폭팔 소리 소스
    private int randomNum;
    private Coroutine checkCoroutine = null;

    private void Awake()
    {
        randomNum = Random.Range(2, 6);
        animator = GetComponent<Animator>(); // 초기화
        audioSource = GetComponent<AudioSource>(); // 초기화
    }

    // 재 생성시 실행할 로직
    public void OnObjectSpanw()
    {
        audioSource.clip = SoundManager.Instance.FXSounds[randomNum]; // 각각의 랜덤한 오디오 클립 생성
        animator.SetTrigger("isDie"); // 애니메이션 재생
        audioSource.Play(); // 재생
        checkCoroutine = StartCoroutine(CheckEndSound()); // 사운드 종료 체크
        //Debug.Log("FX 스크립트 OnObjectSpawn()이 실행됐습니다.");
        //Debug.Log(audioSource.isPlaying);
    }


    #region CheckEndSound() 소리 종료 시점 체크
    IEnumerator CheckEndSound()
    {
        // 무한루프
        while(true)
        {
            // 사운드가 끝났을 때
            if (!audioSource.isPlaying)
            {
                gameObject.SetActive(false); // 오브젝트 비활성화
                StopCoroutine(checkCoroutine); // 코루틴 정지
            }
            yield return new WaitForSeconds(1f); // 사운드가 재생중이면 1초후 다시 체크
            Debug.Log("체크중");
        }
    }
    #endregion
}