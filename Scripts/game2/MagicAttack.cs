using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : StateMachineBehaviour
{
    AICtrl aiCtrl;
    Transform transform;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        if (aiCtrl == null) aiCtrl = animator.GetComponent<AICtrl>();
        if (transform == null) transform = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (aiCtrl.attackTarget == null) return;

        Vector3 aiToPlayerVector = Vector3.ProjectOnPlane(aiCtrl.attackTarget.position - transform.position, Vector3.up);
        Quaternion vectorRotation = Quaternion.LookRotation(aiToPlayerVector);
        Quaternion targetRotation = new Quaternion(0, vectorRotation.y, 0, vectorRotation.w);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
    }
    
    
}
