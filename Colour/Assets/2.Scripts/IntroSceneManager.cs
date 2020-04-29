using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class IntroSceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector; // 플레이어 인트로

    void Start()
    {
        playableDirector.Play(); // 씬 플레이
    }

    void Update()
    {
        // 타임라인이 끝나면
        if (playableDirector.time > 10.6f )
        {
            // 다음씬 로드
            SceneManager.LoadScene(1);
        }
    }
}
