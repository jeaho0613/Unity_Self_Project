using UnityEngine;

public class FX : MonoBehaviour,IPooledObject
{
    public Animator animator; // 오브젝트 애니메이터

    private void Awake()
    {
        //Debug.Log("FX 스크립트 Awake()가 실행됐습니다.");
        animator = GetComponent<Animator>();
    }

    public void OnObjectSpanw()
    {
        //Debug.Log("FX 스크립트 OnObjectSpawn()이 실행됐습니다.");
        animator.SetTrigger("isDie");
    }
}
