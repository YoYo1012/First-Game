using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCharacter : MonoBehaviour
{
    [SerializeField] float jumpPower = 8;
    [SerializeField] float gravityMultiplier = 2;
    [SerializeField] float groundCheckDistance = 0.4f;

    Rigidbody rigidbody;
    Animator animator;

    bool isOnGround;
    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 move, bool isJump) {
        float checkDistance = rigidbody.velocity.y < 0.01f ? groundCheckDistance : 0.01f;
        Vector3 groundNormal = Vector3.up;
        isOnGround = false;
        if(Physics.Raycast(transform.position + Vector3.up * 0.15f, Vector3.down, out RaycastHit hit, checkDistance)) {
            isOnGround = true;
            groundNormal = hit.normal;
        }

        if (isOnGround) {
            if (isJump) {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
        else {
            rigidbody.AddForce((Physics.gravity * gravityMultiplier) - Physics.gravity);
        }

        move = Vector3.ProjectOnPlane(move, groundNormal);
        move = transform.InverseTransformDirection(move);
        float turnSpeed = Mathf.Lerp(180, 360, move.z);
        transform.Rotate(0, Mathf.Atan2(move.x, move.z) * turnSpeed * Time.deltaTime, 0);

        animator.SetFloat("Speed", move.z, 0.1f, Time.deltaTime);
        animator.SetBool("Ground", isOnGround);
        animator.SetFloat("vSpeed", rigidbody.velocity.y);
    }

    private void OnAnimatorMove() {
        if (isOnGround) {
            Vector3 v = animator.deltaPosition / Time.deltaTime;
            v.y = rigidbody.velocity.y;
            rigidbody.velocity = v;
        }
    }
}
