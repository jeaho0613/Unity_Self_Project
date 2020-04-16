using UnityEngine;
using DG.Tweening;

public class Bar : MonoBehaviour, IPooledObject
{
    public string currentType; // Bar의 Type

    public void OnObjectSpanw()
    {
        transform.DOMove(new Vector3(0, -5.5f, 0), 5f).SetEase(Ease.Linear);
    }

    // Bar가 collider에 부딪쳤을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        // player에 부딪칠 때
        if (other.tag == "Player")
        {
            // playerControll을 가져옴
            var otherCom = other.GetComponent<PlayerControll>();
            // 현재 currentColor의 값
            string currentColor = otherCom.currentColor;
            // 현재 bar의 색이랑 비교함
            // - 다르면 player 비활성화, 게임 오버
            if (!(currentColor == currentType))
            {
                otherCom.isDie = true; // 캐릭터를 사망으로
                SoundManager.Instance.GetComponent<AudioSource>().Stop(); // 배경음 정지
                other.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.endGame); // 사망 효과음 발생
            }
        }

        // Wall에 부딪칠 때
        if (other.tag == "Wall")
        {
            // bar 비활성화
            gameObject.SetActive(false);
        }
    }
}
