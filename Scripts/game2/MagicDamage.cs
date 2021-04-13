using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour
{
    [SerializeField] float damage = 250;

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collision.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
