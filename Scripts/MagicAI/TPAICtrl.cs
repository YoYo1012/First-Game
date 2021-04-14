using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TPAICtrl : MonoBehaviour
{
    [HideInInspector] public Vector3 destPosition;//AI隨機移動目的地
    [HideInInspector] public Transform attackTarget;//AI攻擊目標
    public LayerMask whatIsPlayer;
    public float seeRange = 20;//偵測玩家距離
    public float atkRange = 12.5f;//開始攻擊距離
    public float safeRange = 5;//保持安全距離
    [SerializeField] float seeAngle = 120;//AI可視角度
    [SerializeField] GameObject fireballOnHand;//手上火球預置物

    TPCharacter character;
    [HideInInspector] public NavMeshAgent agent;
    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<TPCharacter>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = true;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (agent.enabled) {
            agent.SetDestination(destPosition);

            if (agent.remainingDistance > agent.stoppingDistance) {//剩餘距離大於停止範圍半徑
                character.Move(agent.desiredVelocity, false);//傳送期望速度Vector3值
            }
            else {
                character.Move(Vector3.zero, false);
            }
        }
    }
    
    void DisableNavMeshAgent() {
        agent.enabled = false;
    }

    private void OnDrawGizmos() {//場景輔助，必須寫在Method下
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, seeRange);//形狀,起點,長度
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeRange);

        Gizmos.color = Color.white;//畫出視角範圍
        Vector3 p1 = Quaternion.Euler(0, seeAngle / 2, 0) * transform.forward * seeAngle + transform.position;
        Vector3 p2 = Quaternion.Euler(0, -seeAngle / 2, 0) * transform.forward * seeAngle + transform.position;
        Gizmos.DrawLine(transform.position, p1);
        Gizmos.DrawLine(transform.position, p2);
    }

    void CreateFireball() {
        fireballOnHand.SetActive(true);//生成手上火球
    }

    void ShootFireball(GameObject fireballPrefab) {
        fireballOnHand.SetActive(false);
        Instantiate(fireballPrefab, fireballOnHand.transform.position,
            Quaternion.LookRotation(attackTarget.position + Vector3.up * 1.3f - fireballOnHand.transform.position));//從手上往目標方向移動(Look目標和手的向量)
    }

    void CreateFireMeteor(GameObject fieMeteorPrefab) {//生成大招
        Vector3 vector = new Vector3Int(20, 0, 20);
        Instantiate(fieMeteorPrefab, attackTarget.position + vector, Quaternion.identity);
    }
}
