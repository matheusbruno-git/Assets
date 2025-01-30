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
        public LikingType likingType;
        public string value;
    }

    [System.Serializable]
    public class Disliking
    {
        public DislikingType dislikingType;
        public string value;
    }

    public List<Liking> likings = new List<Liking>();
    public List<Disliking> dislikings = new List<Disliking>();
    public List<string> memories = new List<string>();

    public void AddToMemories(string memory)
    {
        if (!memories.Contains(memory))
        {
            memories.Add(memory);
            Debug.Log($"Memory added: {memory}");
        }
    }
}
