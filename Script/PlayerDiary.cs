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
    private List<Quests> questTextObjects = new List<Quests>();
    public PlayerData playerData;

    public GameObject mapObject;
    public Image mapImage;

    // Other important things
    public Text otherText;

    private void Start()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerData != null)
        {
            playerDiaryText.text = playerData.playerDiaryText;
            otherText.text = playerData.otherText;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerDiary.gameObject.SetActive(!playerDiary.gameObject.activeSelf);
            if (playerDiary.gameObject.activeSelf)
            {
                for (int i = 0; i < questTextObjects.Count; i++)
                {
                    questTextObjects[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < questTextObjects.Count; i++)
                {
                    questTextObjects[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShowQuests(List<string> quests)
    {
        playerDiary.gameObject.SetActive(true);
        for (int i = 0; i < quests.Count; i++)
        {
            GameObject questObject = Instantiate(questText.gameObject, questLayoutGroup.transform);
            Quests quest = questObject.GetComponent<Quests>();
            quest.QuestName.text = quests[i];
            questTextObjects.Add(quest);
        }
    }

    public void ShowMap()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(true);
        }
    }

    public void HideMap()
    {
        if (mapObject != null)
        {
            mapObject.SetActive(false);
        }
    }
}

