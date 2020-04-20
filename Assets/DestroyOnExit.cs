using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.tag == "Enemy")
            Destroy(animator.transform.parent.gameObject, stateInfo.length);
        else
            Destroy(animator.gameObject, stateInfo.length);
    }
}
