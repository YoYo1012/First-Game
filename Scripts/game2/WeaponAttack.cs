using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponAttack : MonoBehaviour
{
    float damage = 150;//傷害
    public AudioSource cut_in;//武器聲

    [SerializeField] GameObject damageTextPrefab;//生成顯示傷害的預置物
    Transform damageTextTransform;//預置物位置

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.root.GetComponent<Animator>();
        damageTextTransform = GameObject.Find("damagePos").GetComponent<Transform>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {//碰撞layer=Enemy時
            if (Mathf.Approximately(animator.GetFloat("AttackCurve"), 1)) {//動畫曲線為1時才有傷害
                other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);//傳送傷害給血量腳本
                Instantiate(damageTextPrefab, damageTextTransform.position, Quaternion.identity,damageTextTransform.parent);
                cut_in.Play();
            }
            
        }
    }

    public float Atk()
    {
        return damage;
    }
}
