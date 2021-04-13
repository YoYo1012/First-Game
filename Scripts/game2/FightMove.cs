using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FightState { LookAt, Close, Attack, Back }

public class FightMove : StateMachineBehaviour
{
    FightState state;
    float actionEndTime;
    AICtrl aiCtrl;
    Transform transform;
    bool isActionInit;//初始化
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (aiCtrl == null) aiCtrl = animator.GetComponent<AICtrl>();
        if (transform == null) transform = animator.transform;

        actionEndTime = Time.time + 1.5f;
        isActionInit = false;

        float aiToPlayerDistance = Vector3.Distance(transform.position, aiCtrl.attackTarget.position);//AI跟目標的距離
        if(aiToPlayerDistance < aiCtrl.atkRange) {
            if(aiToPlayerDistance < aiCtrl.safeRange) {
                actionEndTime = Time.time + 3;
                state = FightState.Back;
            }
            else {
                state = FightState.Attack;
            }
        }
        else {
            if (Random.Range(0, 100) % 2 == 0) {
                state = FightState.LookAt;
            }
            else {
                state = FightState.Close;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Time.time > actionEndTime && (state == FightState.LookAt || state == FightState.Close || state == FightState.Back)) {
            animator.SetTrigger("Exit");//定時清除動作，避免AI重複動作，攻擊狀態除外
        }
        else {
            if (aiCtrl.attackTarget == null) return;//檢查是否有攻擊目標

            if ((aiCtrl.whatIsPlayer.value & (1 << aiCtrl.attackTarget.gameObject.layer)) == 0) {//不對場景攻擊(場景設置Default = layer = 0)
                aiCtrl.attackTarget = null;
                animator.SetBool("IsFighting", false);
                return;
            }

            float aiToPlayerDistance = Vector3.Distance(transform.position, aiCtrl.attackTarget.position);
            if(aiToPlayerDistance > aiCtrl.seeRange ) {//玩家不再AI視野內，無攻擊目標
                aiCtrl.attackTarget = null;
                animator.SetBool("IsFighting", false);
            }
            else {
                switch (state) {
                    case FightState.LookAt://進入視線盯緊玩家
                        if (!isActionInit) {
                            isActionInit = true;
                            aiCtrl.agent.speed = 0;
                        }
                        LookPlayer(); 
                        break;
                    case FightState.Close://往目標移動
                        if (!isActionInit) {
                            isActionInit = true;
                            aiCtrl.agent.speed = 0.5f;
                        }
                        aiCtrl.destPosition = aiCtrl.attackTarget.position;
                        LookPlayer();
                        break;
                    case FightState.Attack://攻擊
                        aiCtrl.agent.speed = 0;
                        if (LookPlayer()) {//確認AI是否面向玩家
                            if (!isActionInit) {
                                isActionInit = true;
                                if (Random.Range(0, 100) % 7 == 0) { //隨機決定攻擊方式
                                    animator.SetInteger("MagicType", 2);
                                }
                                else {
                                    animator.SetInteger("MagicType", 1);
                                }
                                animator.SetTrigger("Attack");
                            }
                        }
                        break;
                    case FightState.Back://後退
                        if (!isActionInit) {
                            isActionInit = true;
                            Vector3 backPosition = (transform.position - aiCtrl.attackTarget.position)
                                * aiCtrl.safeRange * 1.5f + transform.position;//要遠離目標的距離
                            if (NavMesh.SamplePosition(backPosition, out NavMeshHit hit, Vector3.Distance(transform.position, backPosition) - 1, -1)) {
                                //查詢距離內最近的點，-1是扣掉導航判定的半徑
                                aiCtrl.destPosition = hit.position;
                                aiCtrl.agent.speed = 1;
                            }
                            else {
                                state = FightState.Attack;
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
        Quaternion targetRotation = new Quaternion(0, vectorRotation.y, 0, vectorRotation.w);//最終旋轉角度
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);//差值法

        if (Vector3.Angle(transform.forward, aiToPlayerVector) < 0.1f) {//檢查AI是否面向玩家
            return true;
        }
        else return false;
    }
}