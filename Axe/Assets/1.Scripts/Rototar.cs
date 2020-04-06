using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rototar : MonoBehaviour
{
    public float speed; // 회전 힘
    public float starRotate; // y축 회전을 위한 변수
    public bool isTimeStop = false; // TimeStop 스킬 체크

    private void Start()
    {
        // stage3의 MoveCircle 로직
        if (gameObject.tag == "MoveCircle")
        {
            transform.DOLocalMove(new Vector2(0, 38), 5)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
        else if(gameObject.tag == "MoveCircle1")
        {
            transform.DOLocalMove(new Vector2(0, 6.7f), 5)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }    

    }

    void Update()
    {
        // 시간경과 값을 계속 더해줌
        starRotate += Time.deltaTime; 

        // TimeStop 스킬
        if (Input.GetMouseButtonDown(1) && !isTimeStop)
        {
            StartCoroutine(TimeStop());
        }

        // Circle, Star 회전 로직
        if (gameObject.tag != "Star")
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, starRotate * speed, 0);
        }

        // 타임스탑 Skill
        IEnumerator TimeStop()
        {
            isTimeStop = true;
            float save = speed;
            speed = speed / 10;
            yield return new WaitForSeconds(1f);
            speed = save;
            isTimeStop = false;
        }
    }
}
