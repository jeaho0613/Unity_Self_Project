using UnityEngine;

// StateMachineBehaviour 스크립트
// - 애니메이션의 특정 상태를 스크립트로 조정
public class DesetoryBehaviour : StateMachineBehaviour
{
    // 애니메이션이 끝날 떄~
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("끝났습니다.");
        animator.gameObject.SetActive(false); // 애니메이션 종료시 오브젝트 비활성화
    }
}
