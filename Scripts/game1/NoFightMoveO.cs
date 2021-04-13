using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoFightStateO { Idle, Walk }
public class NoFightMoveO : StateMachineBehaviour
{
    NoFightStateO state;
    float actionEndTime;
    TPAICtrl aiCtrl;
    Transform transform;
    bool isActionInit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(aiCtrl == null) aiCtrl = animator.GetComponent<TPAICtrl>();
        if(transform == null) transform = animator.transform;

        actionEndTime = Time.time + 3;
        isActionInit = false;

        if (Random.Range(0, 100) % 2 == 0) {
            state = NoFightStateO.Idle;
        }
        else {
            state = NoFightStateO.Walk;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(Time.time > actionEndTime) {
            animator.SetTrigger("Exit");
        }
        else {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCtrl.seeRange, aiCtrl.whatIsPlayer);
            if (colliders.Length > 0) {
                aiCtrl.attackTarget = colliders[0].transform;
                animator.SetBool("IsFighting", true);
            }
            else {
                if (!isActionInit) {
                    switch (state) {
                        case NoFightStateO.Idle:
                            aiCtrl.agent.speed = 0;
                            break;
                        case NoFightStateO.Walk:
                            aiCtrl.agent.speed = 0.5f;
                            aiCtrl.destPosition = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward * 10 + transform.position;
                            break;
                    }
                    isActionInit = true;
                }
            }
        }
    }
}
