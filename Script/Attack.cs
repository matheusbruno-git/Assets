using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator anim;
    public bool isAttacking;
    public GameObject trail;

    void Start() 
    {
        trail.SetActive(false);
        isAttacking = false;
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            OnClick();
        }
        else 
        {
            StartCoroutine(ResetAttack());
        }
    }

    void OnClick() 
    {
        trail.SetActive(true);
        isAttacking = true;

        anim.SetInteger("AttackN", Random.Range(0, 2));
        anim.SetTrigger("Attack");
    }
    

    IEnumerator ResetAttack() 
    {
        yield return new WaitForSeconds(3);

        isAttacking = false;
        trail.SetActive(false);
    }
}
