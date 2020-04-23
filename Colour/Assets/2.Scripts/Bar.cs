using UnityEngine;
using DG.Tweening;

public class Bar : MonoBehaviour, IPooledObject
{
    public string currentType; // Bar의 Type

    // 재 생성시 실행할 로직
    public void OnObjectSpanw()
    {
        transform.DOMove(new Vector3(0, -5.5f, 0), 1f).SetEase(Ease.Linear); // 생성시 움직임
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
            // - 다르면 playerDie() 함수 실행
            if (currentColor != currentType)
            {
                otherCom.PlayerDie();
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
