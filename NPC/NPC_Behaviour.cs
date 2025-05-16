using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct Personality
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

    [Header("Locations")]
    public Transform JobLocation;
    public Transform homeLocation;

    [Header("Schedule")]
    public float WakeUpTime, LunchTime, EndLunchTime, SocializeTime, GoHomeTime;

    [Header("Stats")]
    public float happiness;
    public float hunger;
    public float hungerIncreaser;

    [Header("References")]
    public Brain CognitiveProfile;
    public LightningManager dayScript;

    private bool IsEating;
    private bool isTalking = false;

    [Header("Dialogue")]
    public float detectionRadius = 5f;

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

        var foodTags = BrainCode.EmotionalAssociations;
        string favoriteFood = null;
        int bestWeight = int.MinValue;

        foreach (var tag in foodTags)
        {
            if (tag.Tag.StartsWith("Food_") && tag.Weight > bestWeight)
            {
                bestWeight = tag.Weight;
                favoriteFood = tag.Tag.Substring(5); // remove "Food_"
            }
        }

        if (!string.IsNullOrEmpty(favoriteFood))
        {
            Eat(favoriteFood);
        }
    }

    void Eat(string food)
    {
        anim.Play("Eating");
        hunger = 0;
        BrainCode.LogMemory($"Ate {food}");
    }

    void Socialize()
    {
        foreach (var tag in BrainCode.EmotionalAssociations)
        {
            if (tag.Tag.StartsWith("Place_") && tag.Weight > 0)
            {
                string placeName = tag.Tag.Substring(6); // remove "Place_"
                VisitPlace(placeName);
            }
            else if (tag.Tag.StartsWith("Person_") && tag.Weight > 0)
            {
                string personName = tag.Tag.Substring(7);
                InteractWithPerson(personName);
            }
        }
    }

    void VisitPlace(string place)
    {
        Transform placeTransform = GameObject.Find(place)?.transform;
        if (placeTransform != null)
        {
            agent.SetDestination(placeTransform.position);
            BrainCode.LogMemory($"Visited {place}");
        }
    }

    void InteractWithPerson(string personName)
    {
        GameObject target = GameObject.Find(personName);
        if (target != null)
        {
            float talkTime = Random.Range(5f, 60f);
            StartCoroutine(Conversation(target.GetComponent<NPC_Behaviour>(), talkTime));
        }
    }

    void DetectNearbyNPCs()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var col in colliders)
        {
            if (col != GetComponent<Collider>())
            {
                NPC_Behaviour npc = col.GetComponent<NPC_Behaviour>();
                if (npc != null && !npc.isTalking && !isTalking)
                {
                    float talkTime = Random.Range(5f, 60f);
                    StartCoroutine(Conversation(npc, talkTime));
                    break;
                }
            }
        }
    }

    IEnumerator Conversation(NPC_Behaviour other, float duration)
    {
        isTalking = true;
        other.isTalking = true;

        anim.Play("Talk");
        other.anim.Play("Talk");

        yield return new WaitForSeconds(duration);

        isTalking = false;
        other.isTalking = false;

        anim.Play("Idle");
        other.anim.Play("Idle");

        BrainCode.LogMemory($"{name} talked with {other.name} for {duration} seconds");
    }
}
