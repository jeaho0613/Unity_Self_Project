using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class IntroSceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    void Start()
    {
        playableDirector.Play();
    }

    void Update()
    {
        if (playableDirector.time > 10.6f )
        {
            SceneManager.LoadScene(1);
        }
    }
}
