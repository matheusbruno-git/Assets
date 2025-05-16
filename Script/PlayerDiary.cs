using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerDiary : MonoBehaviour
{
    public Canvas playerDiary;
    public Text playerDiaryText;
    public Text questText;
    public VerticalLayoutGroup questLayoutGroup;
    private List<Quest> questList = new List<Quest>();
    public PlayerData playerData;

    public GameObject mapObject;
    public Image mapImage;

    public GameObject questObjectPrefab; // Prefab for the quest object UI

    private void Start()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(false);
        }
    }

    void Update()
    {
        // You can update quest status or player diary content here if necessary
    }

    // Display quests in the diary
    public void ShowQuests()
    {
        // Clear current quests
        foreach (Transform child in questLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        // Add quests to the list
        foreach (Quest quest in questList)
        {
            // Create a new quest entry using the prefab
            GameObject questEntry = Instantiate(questObjectPrefab, questLayoutGroup.transform);

            // Get the QuestObject component from the instantiated prefab
            QuestObject questObject = questEntry.GetComponent<QuestObject>();

            // Set the quest name
            questObject.questNameText.text = quest.QuestName;

            // Set the quest state (Active/Inactive/Completed)
            questObject.questStateText.text = $"State: {quest.missionState}";

            // Set the quest condition (e.g., CollectItem, TalkToNPC, etc.)
            string conditionText = $"Condition: {quest.conditionType} - {quest.targetName} x{quest.targetAmount}";
            questObject.questConditionText.text = conditionText;
        }
    }

    // Show map in the diary
    public void ShowMap()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(true);
        }
    }

    // Hide map in the diary
    public void HideMap()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(false);
        }
    }

    // Optionally add quests to the player diary (for testing or dynamic quest additions)
    public void AddQuest(Quest newQuest)
    {
        questList.Add(newQuest);
    }
}

// QuestObject class for the quest UI entry
public class QuestObject : MonoBehaviour
{
    public Text questNameText;
    public Text questConditionText;
    public Text questStateText;
}
