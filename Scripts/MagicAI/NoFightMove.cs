using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoFightState { Idle, Walk }
public class NoFightMove : StateMachineBehaviour
{
    NoFightState state;
    float actionEndTime;
    AICtrl aiCtrl;
    Transform transform;
    bool isActionInit;//初始化
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(aiCtrl == null) aiCtrl = animator.GetComponent<AICtrl>();//抓AICtrl腳本
        if(transform == null) transform = animator.transform;

        actionEndTime = Time.time + 3;
        isActionInit = false;

        if (Random.Range(0, 100) % 2 == 0) {//隨機決定AI狀態
            state = NoFightState.Idle;
        }
        else {
            state = NoFightState.Walk;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(Time.time > actionEndTime) {//定時清除動作，避免AI重複動作
            animator.SetTrigger("Exit");
        }
        else {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCtrl.seeRange, aiCtrl.whatIsPlayer);//檢查範圍內是否有玩家
            if (colliders.Length > 0) {
                aiCtrl.attackTarget = colliders[0].transform;//指定攻擊目標
                animator.SetBool("IsFighting", true);//切換攻擊狀態
            }
            else {
                if (!isActionInit) {
                    switch (state) {
                        case NoFightState.Idle:
                            aiCtrl.agent.speed = 0;//導航移動速度
                            break;
                        case NoFightState.Walk:
                            aiCtrl.agent.speed = 0.5f;
                            aiCtrl.destPosition = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward * 10 + transform.position;//AI移動目標
                            break;
                    }
                    isActionInit = true;
                }
            }
        }
    }
}
