using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCtrl : MonoBehaviour
{
    [SerializeField] GameObject freeLookCamera;
    Character character;
    bool isDead = false;
    Animator animator;

    bool isJump;//是否跳躍
    public bool isAim;//是否瞄準中
    bool Aim;//瞄準
    Rigidbody moveing;
    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update() {
        if (!isJump) isJump = Input.GetButtonDown("Jump");
        if (Input.GetMouseButtonDown(0))//按左鍵瞄準
            Aim = true;
        if (Input.GetMouseButtonUp(0))//放開相反
            Aim = false;
    

    }
    bool isMove;
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!Aim)//不瞄準時
            {
                Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
                Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1));
                Vector3 move = camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal");//WASD+相機方向移動

                Vector2 movement = Vector2.zero;//角色XY值=0
                character.Move(move, isJump, isAim, movement);//輸入變數給Move
                isJump = false;
                isAim = false;
            }
            else if (Aim)//瞄準時
            {
                Vector2 movement = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                Vector3 move = Vector3.zero;
                isAim = true;
                isJump = false;
                character.Move(move, isJump, isAim, movement);
            }
        }
    }

    void SetDead() {
        isDead = true;
        freeLookCamera.SetActive(false);
    }
}
