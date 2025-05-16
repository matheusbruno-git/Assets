using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBrain", menuName = "Scriptables/Brain", order = 2)]
public class Brain : ScriptableObject
{
    [System.Serializable]
    public struct EmotionTag
    {
        public string Tag;
        public int Weight;
    }

    [System.Serializable]
    public struct TimedMemory
    {
        public string Description;
        public float TimeStamp;
    }

    [Header("Emotion Tags")]
    [SerializeField]
    public List<EmotionTag> emotionalAssociations = new List<EmotionTag>();

    [Header("Memories")]
    [SerializeField]
    public List<TimedMemory> memories = new List<TimedMemory>();

    public void RegisterEmotion(string tag, int weight)
    {
        int index = emotionalAssociations.FindIndex(e => e.Tag == tag);
        if (index >= 0)
        {
            emotionalAssociations[index] = new EmotionTag { Tag = tag, Weight = weight };
        }
        else
        {
            emotionalAssociations.Add(new EmotionTag { Tag = tag, Weight = weight });
        }
    }

    public void LogMemory(string description)
    {
        float now = Time.time;
        memories.Add(new TimedMemory { Description = description, TimeStamp = now });
        Debug.Log($"Memory added: {description} at time {now}");
    }

    public List<EmotionTag> EmotionalAssociations => emotionalAssociations;
    public List<TimedMemory> Memories => memories;
}
