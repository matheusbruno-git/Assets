using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public Animator anim;
    private NavMeshAgent agent;
    public Rigidbody rb;
    private bool CanGetHurted = true;

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float amount)
    {
        if(CanGetHurted) 
        {
            Debug.Log("Hurt");
            anim.Play("Impact");
            health -= amount;
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Takedown()
    {
        anim.Play("Takedown");
        Die();
    }

    void Die()
    {
        CanGetHurted = false;
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>()) 
        {
            rb.AddForce(-transform.forward, ForceMode.Impulse);
        }
        anim.SetBool("Died", true);
        Destroy(this.gameObject, 5f);
    } 
}
