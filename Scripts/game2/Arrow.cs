using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float damage = 200;//弓箭傷害

    [SerializeField] GameObject damageTextPrefab;//顯示傷害值
    Transform damageTextTransform;

    public float force;
    public Rigidbody rigidbody;

    AudioSource hit;//弓箭擊中的聲音
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        damageTextTransform = GameObject.Find("damagePos").GetComponent<Transform>();
        hit = GameObject.Find("BloodBlade1").GetComponent<AudioSource>();
    }

    public void Fire()
    {
        rigidbody.AddForce(transform.forward * (100 * Random.Range(1.3f, 1.7f)), ForceMode.Impulse);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            Instantiate(damageTextPrefab, damageTextTransform.position,
                Quaternion.identity, damageTextTransform.parent);
            StartCoroutine(Countdown());
        }
        hit.Play();
        StartCoroutine(Countdown());

    }
    IEnumerator Countdown()//碰撞物體3秒後移除弓箭
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public float Atk()
    {
        return damage;//傳送傷害值給顯示的Text
    }
}
