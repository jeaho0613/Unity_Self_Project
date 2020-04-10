using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rototar : MonoBehaviour
{
    public float speed; // 회전 힘
    public float starRotate; // y축 회전을 위한 변수
    public bool isTimeStop = false; // TimeStop 스킬 체크
    public bool delay = false;

    public SoundManger soundManger;

    private void Start()
    {
        switch (gameObject.tag)
        {
            case "MoveCircle":
                transform.DOLocalMove(new Vector2(0, 38), 5)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
                break;
            case "MoveCircle1":
                transform.DOLocalMove(new Vector2(0, 6.7f), 5)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
                break;
            case "Star":
                transform.DORotate(new Vector3(0, 180, 0), 4)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
                break;
        }
    }

    private void Update()
    {
        // Circle 이거나, MoveCircle 일때
        if (gameObject.tag == "Circle" || gameObject.tag == "MoveCircle")
        {
            // 회전
            gameObject.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        }

        // Flsh 스킬
        // - W버튼 클릭 && 쿨타임 && Stage가 1이상일때 && count가 남아있을 때
        if (Input.GetKeyDown(KeyCode.W) && !isTimeStop && (GameManager.Instance.LoadPoint > 0))
        {
            StartCoroutine(TimeStop(2f)); // TimeStop 코루틴 사용
        }
    }

    // 타임스탑 Skill
    IEnumerator TimeStop(float cool)
    {
        isTimeStop = true; // 스킬 실행중
        float save = speed; // 원래 speed값 세이브
        speed = speed / 10; // speed를 감소
        yield return new WaitForSeconds(1f); // 스킬 지속효과 시간
        speed = save; // 다시 속도를 원래값으로 돌려주고
        yield return new WaitForSeconds(cool); // 스킬 쿨타임 (cool 시간)
        isTimeStop = false; // 스킬 종료
    }
}
