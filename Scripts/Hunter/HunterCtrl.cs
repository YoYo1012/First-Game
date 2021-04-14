using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HunterCtrl : MonoBehaviour //弓箭控制
{
    Animator animator;
    UserCtrl ctrl;
    public Transform arrowPos;//弓箭位置
    public GameObject arrowPrefab, arrow;//發射的箭&原弓箭

    public GameObject look;//發射方向

    [SerializeField] GameObject freeCamera, mainCameraCtrl, front_sight;//跟隨相機/視角相機控制/準星Image
    private void Start()
    {
        animator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    // Update is called once per frame
    private void Update()
    {       
        if (Input.GetMouseButtonDown(0))//按住左鍵開始瞄準
        {
            arrow.gameObject.SetActive(true);//顯示手上的弓箭
            animator.SetBool("IsAim", true);//切換瞄準動作
            front_sight.SetActive(true);//顯示準星
            freeCamera.SetActive(false);
            mainCameraCtrl.SetActive(true);//關掉跟隨相機並啟動視角控制
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Fire");
            FireArrow();//射擊
            arrow.gameObject.SetActive(false);
            animator.SetBool("IsAim", false);
                     
            StartCoroutine(delayCamera());//協程
        }

    }

    IEnumerator delayCamera()
    {
        mainCameraCtrl.SetActive(false);
        front_sight.SetActive(false);
        yield return new WaitForSeconds(1);//延遲1秒再開啟跟隨，避免畫面切換過快
        freeCamera.SetActive(true);
    }
    void FireArrow()
    {
        GameObject projectile = Instantiate(arrowPrefab);//生成弓箭
        projectile.transform.forward = look.transform.forward;//將弓箭方向指向視角方向
        projectile.transform.position = arrowPos.position + look.transform.forward;//射出的箭位置=手上箭的位置
        //Wait for the position to update
        projectile.GetComponent<Arrow>().Fire();//執行程式Arrow的Fire()Method

    }
}


