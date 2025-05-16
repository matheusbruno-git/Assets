using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CognitiveProfile", menuName = "Scriptables/CognitiveProfile", order = 3)]
public class CognitiveProfile : ScriptableObject
{
    [Serializable]
    public class EmotionTag
    {
        public string Tag;
        [Range(-100, 100)] public int Weight;

        public EmotionTag(string tag, int weight)
        {
            Tag = tag;
            Weight = Mathf.Clamp(weight, -100, 100);
        }
    }

    [Serializable]
    public class TimedMemory
    {
        public string Description;
        public DateTime Timestamp;

        public TimedMemory(string description)
        {
            Description = description;
            Timestamp = DateTime.Now;
        }
    }

    [SerializeField] private List<EmotionTag> emotionalAssociations = new();
    [SerializeField] private List<TimedMemory> memoryLog = new();

    public IReadOnlyList<EmotionTag> EmotionalAssociations => emotionalAssociations.AsReadOnly();
    public IReadOnlyList<TimedMemory> MemoryLog => memoryLog.AsReadOnly();

    public void RegisterEmotion(string tag, int weight)
    {
        if (string.IsNullOrWhiteSpace(tag)) return;

        var existing = emotionalAssociations.Find(e => e.Tag == tag);
        if (existing != null)
        {
            existing.Weight = Mathf.Clamp(existing.Weight + weight, -100, 100);
            Debug.Log($"Updated emotion tag '{tag}' to {existing.Weight}");
        }
        else
        {
            emotionalAssociations.Add(new EmotionTag(tag, weight));
            Debug.Log($"Added new emotion tag '{tag}' with weight {weight}");
        }
    }

    public void LogMemory(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        memoryLog.Add(new TimedMemory(description));
        Debug.Log($"Memory logged: '{description}' at {DateTime.Now}");
    }

    public int GetEmotionWeight(string tag)
    {
        var emotion = emotionalAssociations.Find(e => e.Tag == tag);
        return emotion?.Weight ?? 0;
    }

    public void ClearAllData()
    {
        emotionalAssociations.Clear();
        memoryLog.Clear();
        Debug.Log("CognitiveProfile reset.");
    }
}
