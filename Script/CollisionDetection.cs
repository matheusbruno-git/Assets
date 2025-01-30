using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public CombatManager attackCode;
    public Target target;
    public float damage;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy" && attackCode.isAttacking) 
        {
            Debug.Log(other.name);
            Target target = other.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

}
