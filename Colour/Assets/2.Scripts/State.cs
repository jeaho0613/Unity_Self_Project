using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class State : StateMachineBehaviour
{
    // 애니메이션이 끝나면~
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.GameEnd(1);
        animator.transform.DOScale(0, 0);
    }
}
