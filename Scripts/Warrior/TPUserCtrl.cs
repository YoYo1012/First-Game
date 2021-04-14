using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPUserCtrl : MonoBehaviour
{
    [SerializeField] GameObject freeLookCamera;
    TPCharacter character;
    bool isDead = false;//人物是否死亡
    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<TPCharacter>();
    }

    bool isJump;
    private void Update() {
        if (!isJump) isJump = Input.GetButtonDown("Jump");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isDead) {//保護
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));//scale:縮放
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1));
            Vector3 move = camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal");

            character.Move(move.normalized, isJump);
            isJump = false;
        }
    }

    void SetDead() {//人物死往停止相機和控制
        isDead = true;
        freeLookCamera.SetActive(false);
    }
}
