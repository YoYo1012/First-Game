using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCtrl : MonoBehaviour
{
    [SerializeField] float maxHp = 1000;
    [SerializeField] Image hpImage;//血量圖
    [SerializeField] bool isPlayerUse = false;
    [SerializeField] Text hpText;//血量值

    Rigidbody rootRigidbody;
    Collider rootCollider;
    Animator animator;

    [HideInInspector] public float currHp;

    Rigidbody[] rigidbodies;
    Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        currHp = maxHp;
        rootRigidbody = GetComponent<Rigidbody>();//原剛體和碰撞器
        rootCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        rigidbodies = GetComponentsInChildren<Rigidbody>();//Ragdoll剛體&碰撞器
        colliders = GetComponentsInChildren<Collider>();
       
        SetRagdoll(false);

    }

    void SetRagdoll(bool active) {//布娃娃
        rootRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;//改變碰撞檢測
        rootRigidbody.isKinematic = active;//靜止剛體
        rootCollider.isTrigger = active;//原剛體&碰撞:忽略碰撞和可穿透
        animator.enabled = !active;//停止動畫

        for(int i=0; i<rigidbodies.Length; i++) {//啟用布娃娃的鋼體和不穿透來癱軟腳色
            if (rigidbodies[i] == rootRigidbody) continue;//檢查並更改
            rigidbodies[i].isKinematic = !active;
        }

        for(int i=0; i<colliders.Length; i++) {
            if (colliders[i] == rootCollider) continue;
            colliders[i].isTrigger = !active;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, currHp / maxHp, 0.1f);//生命的量和值
        hpText.text = "Hp" +currHp;
    }

void Damage(float d) {
        currHp -= d;
        if (currHp <= 0) {
            SetRagdoll(true);//啟動布娃娃
            SendMessage("DisableNavMeshAgent", SendMessageOptions.DontRequireReceiver);//傳送訊息給腳本去停止導航
            SendMessage("SetDead", SendMessageOptions.DontRequireReceiver);//傳送訊息給腳本去告知死亡
            if (isPlayerUse) {
                gameObject.layer = 0;
            }
            else {
                hpImage.transform.parent.gameObject.SetActive(false);
                
            }
        }

        if(currHp < 0) {
            currHp = 0;
        }
    }
}
