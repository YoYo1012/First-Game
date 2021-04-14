using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour//角色移動
{
    [SerializeField] float jumpPower = 8;//跳躍力
    [SerializeField] float gravityMultiplier = 2;//重力
    [SerializeField] float groundCheckDistance = 0.4f;//檢查是否在地上範圍

    Rigidbody rigidbody;
    Animator animator;

    bool isOnGround;
    [SerializeField] GameObject lookAt;//視角
    //[SerializeField] AudioSource foot_steps;


    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    public void Move(Vector3 move, bool isJump,bool isAim,Vector2 movement) {
        float checkDistance = rigidbody.velocity.y < 0.01f ? groundCheckDistance : 0.01f;//如果向下速度小於0.01,則 gCD= 0.01
        Vector3 groundNormal = Vector3.up;
        isOnGround = false;
        if(Physics.Raycast(transform.position + Vector3.up * 0.15f, Vector3.down, out RaycastHit hit, checkDistance)) {//射線從角色往地下打
            isOnGround = true;
            groundNormal = hit.normal;
        }

        if (isOnGround) {
            if (isJump) {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);//跳 = 往 y 軸施加力
            }
        }
        else {
            rigidbody.AddForce((Physics.gravity * gravityMultiplier) - Physics.gravity);
        }
        if (isAim)
        {
            animator.SetFloat("AimX", movement.y);//左右
            animator.SetFloat("AimY", movement.x);//前後
            animator.SetFloat("magnitude", movement.magnitude);//magnitude = 平方開根號

            Vector3 target = lookAt.transform.forward;
            Quaternion turn = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Euler(0, turn.eulerAngles.y, 0);//瞄準時角色跟隨視角旋轉
        }
        else if (!isAim)
        {
            move = Vector3.ProjectOnPlane(move, groundNormal);//平面投影
            move = transform.InverseTransformDirection(move);//世界座標轉 Local座標
            float turnSpeed = Mathf.Lerp(180, 360, move.z);
            transform.Rotate(0, Mathf.Atan2(move.x, move.z) * turnSpeed * Time.deltaTime, 0);

            animator.SetFloat("Speed", move.z, 0.1f, Time.deltaTime);//設置動畫Speed值 = Z軸的值
        }
        animator.SetBool("Ground", isOnGround);
        animator.SetFloat("vSpeed", rigidbody.velocity.y);//Set重力值給Animator
    }

    private void OnAnimatorMove() {
        if (isOnGround) {
            Vector3 v = animator.deltaPosition / Time.deltaTime;
            v.y = rigidbody.velocity.y;
            rigidbody.velocity = v;
        }
    }
}
