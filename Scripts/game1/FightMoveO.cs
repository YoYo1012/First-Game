using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FightStateO { LookAt, Close, Attack, Back }

public class FightMoveO : StateMachineBehaviour
{
    FightStateO state;
    float actionEndTime;
    TPAICtrl aiCtrl;
    Transform transform;
    bool isActionInit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (aiCtrl == null) aiCtrl = animator.GetComponent<TPAICtrl>();
        if (transform == null) transform = animator.transform;

        actionEndTime = Time.time + 1.5f;
        isActionInit = false;

        float aiToPlayerDistance = Vector3.Distance(transform.position, aiCtrl.attackTarget.position);
        if(aiToPlayerDistance < aiCtrl.atkRange) {
            if(aiToPlayerDistance < aiCtrl.safeRange) {
                actionEndTime = Time.time + 3;
                state = FightStateO.Back;
            }
            else {
                state = FightStateO.Attack;
            }
        }
        else {
            if (Random.Range(0, 100) % 2 == 0) {
                state = FightStateO.LookAt;
            }
            else {
                state = FightStateO.Close;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Time.time > actionEndTime && (state == FightStateO.LookAt || state == FightStateO.Close || state == FightStateO.Back)) {
            animator.SetTrigger("Exit");
        }
        else {
            if (aiCtrl.attackTarget == null) return;

            if ((aiCtrl.whatIsPlayer.value & (1 << aiCtrl.attackTarget.gameObject.layer)) == 0) {
                aiCtrl.attackTarget = null;
                animator.SetBool("IsFighting", false);
                return;
            }

            float aiToPlayerDistance = Vector3.Distance(transform.position, aiCtrl.attackTarget.position);
            if(aiToPlayerDistance > aiCtrl.seeRange ) {
                aiCtrl.attackTarget = null;
                animator.SetBool("IsFighting", false);
            }
            else {
                switch (state) {
                    case FightStateO.LookAt:
                        if (!isActionInit) {
                            isActionInit = true;
                            aiCtrl.agent.speed = 0;
                        }
                        LookPlayer(); 
                        break;
                    case FightStateO.Close:
                        if (!isActionInit) {
                            isActionInit = true;
                            aiCtrl.agent.speed = 0.5f;
                        }
                        aiCtrl.destPosition = aiCtrl.attackTarget.position;
                        LookPlayer();
                        break;
                    case FightStateO.Attack:
                        aiCtrl.agent.speed = 0;
                        if (LookPlayer()) {
                            if (!isActionInit) {
                                isActionInit = true;
                                if (Random.Range(0, 100) % 7 == 0) {
                                    animator.SetInteger("MagicType", 2);
                                }
                                else {
                                    animator.SetInteger("MagicType", 1);
                                }
                                animator.SetTrigger("Attack");
                            }
                        }
                        break;
                    case FightStateO.Back:
                        if (!isActionInit) {
                            isActionInit = true;
                            Vector3 backPosition = (transform.position - aiCtrl.attackTarget.position) * aiCtrl.safeRange * 1.5f + transform.position;
                            if (NavMesh.SamplePosition(backPosition, out NavMeshHit hit, Vector3.Distance(transform.position, backPosition) - 1, -1)) {
                                aiCtrl.destPosition = hit.position;
                                aiCtrl.agent.speed = 1;
                            }
                            else {
                                state = FightStateO.Attack;
                                isActionInit = false;
                            }
                        }
                        break;
                }
            }
        }
    }

    bool LookPlayer() {
        Vector3 aiToPlayerVector = Vector3.ProjectOnPlane(aiCtrl.attackTarget.position - transform.position, Vector3.up);
        Quaternion vectorRotation = Quaternion.LookRotation(aiToPlayerVector);
        Quaternion targetRotation = new Quaternion(0, vectorRotation.y, 0, vectorRotation.w);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

        if (Vector3.Angle(transform.forward, aiToPlayerVector) < 0.1f) {
            return true;
        }
        else return false;
    }
}