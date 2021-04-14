using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] float damage = 150;

    [SerializeField] GameObject damageTextPrefab;//傷害顯示預置物
    Transform damageTextTransform;//傷害顯示位置
    // Start is called before the first frame update
    void Start()
    {
        
        damageTextTransform = GameObject.Find("damagePos").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            Instantiate(damageTextPrefab, damageTextTransform.position, Quaternion.identity, damageTextTransform.parent);

        }
        
    }
}
