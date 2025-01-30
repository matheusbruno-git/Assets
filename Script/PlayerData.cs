using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData
{
    public Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    public List<string> talkedToNPCs = new List<string>();
    public List<string> reachedLocations = new List<string>();
    public Dictionary<string, int> defeatedEnemies = new Dictionary<string, int>();
    public float Strength = 20f;
    public float Speed = 2.0f;
    public float AngerLevel;
    public float Health = 100f;
    public float MaxHealth = 100f;

    public bool HasItem(string itemName, int amount)
    {
        return collectedItems.ContainsKey(itemName) && collectedItems[itemName] >= amount;
    }

    public bool HasTalkedToNPC(string npcName)
    {
        return talkedToNPCs.Contains(npcName);
    }

    public bool HasReachedLocation(string locationName)
    {
        return reachedLocations.Contains(locationName);
    }

    public bool HasDefeatedEnemy(string enemyName, int amount)
    {
        return defeatedEnemies.ContainsKey(enemyName) && defeatedEnemies[enemyName] >= amount;
    }
}
