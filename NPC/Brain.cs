using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBrain", menuName = "Scriptables/Brain", order = 2)]
public class Brain : ScriptableObject
{
    public enum LikingType
    {
        Food,
        FavoritePlace,
        Person
    }

    public enum DislikingType
    {
        Food,
        FavoritePlace,
        Person
    }

    [System.Serializable]
    public class Liking
    {
        public LikingType Type { get; set; }
        public string Value { get; set; }

        public Liking(LikingType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    [System.Serializable]
    public class Disliking
    {
        public DislikingType Type { get; set; }
        public string Value { get; set; }

        public Disliking(DislikingType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public List<Liking> Likings { get; private set; } = new List<Liking>();
    public List<Disliking> Dislikings { get; private set; } = new List<Disliking>();
    public List<string> Memories { get; private set; } = new List<string>();

    public void AddToMemories(string memory)
    {
        if (!Memories.Contains(memory))
        {
            Memories.Add(memory);
            Debug.Log($"Memory added: {memory}");
        }
    }

    public void AddLiking(LikingType type, string value)
    {
        Likings.Add(new Liking(type, value));
    }

    public void AddDisliking(DislikingType type, string value)
    {
        Dislikings.Add(new Disliking(type, value));
    }
}

