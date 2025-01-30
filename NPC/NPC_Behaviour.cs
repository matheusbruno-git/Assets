using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

struct Personality
{
    public string name;

    public Personality(string name)
    {
        this.name = name;
    }
}

public class NPC_Behaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    public Transform JobLocation;
    public Transform homeLocation;
    public string job;
    public LightningManager dayScript;
    public float WakeUpTime, LunchTime, EndLunchTime, SocializeTime, GoHomeTime;
    public float happiness, hunger, hungerIncreaser;
    public Brain BrainCode;
    bool IsEating;

    [Header("Dialogue")]
    public string[] lines;
    public float detectionRadius = 5f;

    private bool isTalking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        hunger = Mathf.Min(hunger + Time.deltaTime * hungerIncreaser, 100);
        float hour = dayScript.TimeOfDay;
        if (hour >= WakeUpTime && hour <= LunchTime)
        {
            agent.SetDestination(JobLocation.position);
        }
        else if (hour > LunchTime && hour < EndLunchTime)
        {
            Lunch();
            IsEating = true;
        }
        else if (hour >= EndLunchTime && hour < SocializeTime)
        {
            anim.Play("WorkingIdle");
            IsEating = false;
        }
        else if (hour >= SocializeTime && hour <= GoHomeTime)
        {
            Socialize();
        }
        else if (hour > GoHomeTime)
        {
            agent.SetDestination(homeLocation.position);
        }

        DetectNearbyNPCs();
    }

    void Lunch()
    {
        anim.Play("Lunch");
        var foodLiking = BrainCode.likings.FirstOrDefault(l => l.likingType == Brain.LikingType.Food);
        if (foodLiking != null)
        {
            Eat(foodLiking.value);
        }

    }

    void Eat(string food)
    {
        anim.Play("Eating");
        hunger = 0;
    }

    void Socialize()
    {
        foreach (var liking in BrainCode.likings)
        {
            switch (liking.likingType)
            {
                case Brain.LikingType.FavoritePlace:
                    VisitFavoritePlace(liking.value);
                    break;
                case Brain.LikingType.Person:
                    InteractWithPerson(liking.value);
                    break;
            }
        }
    }

    void VisitFavoritePlace(string place)
    {
        Transform favoritePlaceTransform = GameObject.Find(place)?.transform;
        if (favoritePlaceTransform != null)
        {
            agent.SetDestination(favoritePlaceTransform.position);
        }
    }

    void InteractWithPerson(string person)
    {
        GameObject personObject = GameObject.Find(person);
        if (personObject != null)
        {
            StartConversation(personObject.GetComponent<NPC_Behaviour>());
        }
    }

    void DetectNearbyNPCs()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != this.GetComponent<Collider>())
            {
                NPC_Behaviour nearbyNPC = hitCollider.GetComponent<NPC_Behaviour>();

                if (nearbyNPC != null && !isTalking)
                {
                    StartConversation(nearbyNPC);
                    break;
                }
            }
        }
    }

    void StartConversation(NPC_Behaviour otherNPC)
    {
        isTalking = true;
        otherNPC.isTalking = true;

        anim.Play("Talk");
        otherNPC.anim.Play("Talk");

        Debug.Log($"{name} started talking with {otherNPC.name}");

        float talkTime = Random.Range(5f, 60f);
        StartCoroutine(StopConversation(otherNPC, talkTime));
    }

    void memorize(string memory)
    {
        BrainCode.AddToMemories(memory);
    }

    IEnumerator StopConversation(NPC_Behaviour otherNPC, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isTalking = false;
        otherNPC.isTalking = false;

        anim.Play("Idle");
        otherNPC.anim.Play("Idle");
        memorize($"{name} talked with {otherNPC.name} for {waitTime} seconds");

    }

}
