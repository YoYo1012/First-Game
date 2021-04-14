using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICtrl : MonoBehaviour
{
    [HideInInspector] public Vector3 destPosition;
    [HideInInspector] public Transform attackTarget;
    public LayerMask whatIsPlayer;
    public float seeRange = 20;
    public float atkRange = 12.5f;
    public float safeRange = 5;
    [SerializeField] float seeAngle = 120;
    [SerializeField] GameObject fireballOnHand;

    Character character;
    [HideInInspector] public NavMeshAgent agent;
    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<Character>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = true;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (agent.enabled) {
            agent.SetDestination(destPosition);

            if (agent.remainingDistance > agent.stoppingDistance) {
                character.Move(agent.desiredVelocity, false,false,Vector2.zero);
            }
            else {
                character.Move(Vector3.zero, false,false, Vector2.zero);
            }
        }
    }

    void DisableNavMeshAgent() {
        agent.enabled = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, seeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeRange);

        Gizmos.color = Color.white;
        Vector3 p1 = Quaternion.Euler(0, seeAngle / 2, 0) * transform.forward * seeAngle + transform.position;
        Vector3 p2 = Quaternion.Euler(0, -seeAngle / 2, 0) * transform.forward * seeAngle + transform.position;
        Gizmos.DrawLine(transform.position, p1);
        Gizmos.DrawLine(transform.position, p2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(destPosition, 0.2f);
    }

    void CreateFireball() {
        fireballOnHand.SetActive(true);
    }

    void ShootFireball(GameObject fireballPrefab) {
        fireballOnHand.SetActive(false);
        Instantiate(fireballPrefab, fireballOnHand.transform.position,
            Quaternion.LookRotation(attackTarget.position + Vector3.up * 1.3f - fireballOnHand.transform.position));
    }

    void CreateFireMeteor(GameObject fieMeteorPrefab) {
        Instantiate(fieMeteorPrefab, attackTarget.position, Quaternion.identity);
    }
}
