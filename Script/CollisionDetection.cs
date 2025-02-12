using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public CombatManager attackCode;
    public EnemyAI enemyAI;


    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy" && enemyAI.isAttacking && attackCode != null) 
        {
            EnemyAI target = other.GetComponent<EnemyAI>();
            if(target != null)
            {
                target.TakeDamage(attackCode.damage);
            }
        }
        else if(other.tag == "Player" && attackCode.isAttacking && enemyAI != null)
        {
            Debug.Log(other.name);
            CombatManager playerHealth = other.GetComponent<CombatManager>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(enemyAI.damage);
            }
        }
    }

}

