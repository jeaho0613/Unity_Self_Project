using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    public Text text1, text2, text3;
    public AudioSource walkAudio;
    public bool isPlay = false;
    public PlayableDirector PlayableDirector;
    public int finshTime = 5;
    [Range(0.1f, 0.5f)]
    public float coroutineTime;

    void Start()
    {
        // UI 코루틴을 실행합니다.
        // - Main Text의 애니메이션을 담당합니다.
        StartCoroutine(uiCoroutine());
    }

    private void FixedUpdate()
    {
        // 일정 간격으로 sound를 재생함.
        // - isPlay의 값으로 판별
        if (isPlay)
            StartCoroutine(walkSound());
    }

    IEnumerator uiCoroutine()
    {
        // UI의 애니메이션을 담당
        // - yield return textSequence.WaitForCompletion(); 으로 끝나는 시점을 판별
        // - 끝날때 isPlay를 재생시켜서 애니메이션과 소리를 타켓함
        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(text1.DOText("J", 2))
            .Append(text2.DOText("PROJECT", 2))
            .Append(text3.DOText("#1", 2));

        yield return textSequence.WaitForCompletion();
        isPlay = true;

        text2.DOColor(RandomColor(), 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

    }

    IEnumerator walkSound()
    {
        // Player의 걷는 소리를 담당
        // - isPlay의 값을 변환해주어 특정 시간마다 재생시킴
        walkAudio.PlayOneShot(walkAudio.clip);
        isPlay = false;
        yield return new WaitForSeconds(coroutineTime);
        walkAudio.Stop();
        isPlay = true;
    }

    Color RandomColor()
    {
        // 랜덤컬러
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }
}
