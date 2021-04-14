using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrriorCtrl : MonoBehaviour
{
    [SerializeField] Transform weapon;//武器位置
    [SerializeField] Transform weaponSocketOnBack, weaponSocketOnHand;//改變武器到背上或手上
    [SerializeField] GameObject skillPos;//技能生成位置
    public AudioSource cut;//揮武器聲音

    Animator animator;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    float timer = 0;
    bool isPress;
    private void Update()
    {
        if(Input.GetMouseButtonDown(2)) {//按滑鼠滾輪控制拿起或放武器
            if (animator.GetBool("IsFighting")) animator.SetTrigger("PutWeapon");
            else animator.SetTrigger("GetWeapon");
        }

        if(animator.GetBool("IsFighting")) {
            if(Input.GetMouseButtonDown(0)) {    //滑鼠左鍵普通攻擊
                animator.SetTrigger("Attack");
            }
            if (Input.GetMouseButton(0)) {//按住左鍵旋轉攻擊
                timer += Time.deltaTime;
                if (timer > 2)//按住>2秒才做動作，避免和普通攻擊衝突
                    isPress = true;
            }
            if (isPress)
                animator.SetTrigger("Press");

            if (Input.GetMouseButtonUp(0))//放開左鍵回閒置動作
            {
                timer = 0;
                isPress = false;
            }
            if (Input.GetKeyDown(KeyCode.R))//鍵盤R鍵施放技能
                animator.SetTrigger("Skill");
        }       
    }
    void GetWeapon() {
        animator.SetBool("IsFighting", true);//切換成攻擊狀態
        weapon.SetParent(weaponSocketOnHand);//將武器位置移到手上
        weapon.localPosition = Vector3.zero;//位置歸零
        weapon.localRotation = Quaternion.identity;
    }

    void PutWeapon() {
        animator.SetBool("IsFighting", false);
        weapon.SetParent(weaponSocketOnBack);
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.identity;
    }

    void skillEffect (GameObject skillPrefabs)
    {
        GameObject Lava = Instantiate(skillPrefabs, skillPos.transform.position, Quaternion.LookRotation(transform.forward));//生成技能Particle
        Destroy(Lava,2f);//2秒後刪除技能
    }
    void weaponSound()
    {
        cut.Play();//播放揮武器聲音
    }
        
}
