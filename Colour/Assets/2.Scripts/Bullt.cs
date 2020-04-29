using UnityEngine;
using DG.Tweening;

public class Bullt : MonoBehaviour, IPooledObject
{
    [Range(0f, 10f)]
    public float endTime; // 끝나는 시간
    [Range(0f, 20f)]
    public float power; // 총알 힘

    public void OnObjectSpanw()
    {

        // 보스전이 아닐 경우
        if(!GameManager.Instance.isBoss)
        {
            // 생성된 총알의 태그별 로직
            switch (gameObject.tag)
            {
                // 플레이어의 총알일 경우
                case "PBullet":
                    //Debug.Log($"현재 총알의 name은 : {gameObject.name}");
                    transform.DOMoveY(5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;

                // 적 총알일 경우
                case "EBullet":
                    transform.DOMoveY(-5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;
            }
        }

        // 보스전 일경우
        else
        {
            // 오브젝트의 이름으로 스위치
            switch(gameObject.name)
            {
                case "EnemyBulletL":
                    transform.DOMoveY(-5.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;
                     
                case "EnemyBulletM":
                    transform.DOMoveY(-5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;

                // 플레이어 총알
                case "BulletR":
                case "BulletG":
                case "BulletB":
                case "BulletY":
                    transform.DOMoveY(5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;
                case "EnemyBlueBulletL":
                    transform.DOMoveY(-5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;
                case "EnemyBlueBulletS":
                    transform.DOMoveY(-5.5f, endTime).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // 시간지나면 비활성화
                    });
                    break;
            }
        }
        
    }
}
