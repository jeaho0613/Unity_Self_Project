using System.Collections;
using System.Collections.Generic;
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
            // player의 currentColor값을 가져와서 
            string currentColor = other.GetComponent<PlayerControll>().currentColor;
            // 현재 bar의 색이랑 비교함
            // - 다르면 player 비활성화, 게임 오버
            if (!(currentColor == currentType))
            {
                other.gameObject.SetActive(false);
            }
        }

        // Wall에 부딪칠 때
        if (other.tag == "Wall")
        {
            // bar 비활성화
            Debug.Log("벽에 부딪쳤습니다.");
            gameObject.SetActive(false);
        }
    }
}
