
using System.Collections.Generic;
using UnityEngine;

public enum MissionState
{
    Inactive,
    Active,
    Completed
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Scriptables/Quest", order = 3)]
public class Quest : ScriptableObject
{
    public string QuestName;
    [TextArea] public string QuestDescription;

    [Header("Quest Conditions")]
    public List<QuestCondition> questConditions;

    [Header("Quest Status")]
    public MissionState missionState = MissionState.Inactive;

    [Header("Related NPCs")]
    public NPC_Behaviour[] EmittingNPCs;

    public void ActivateQuest()
    {
        if (missionState == MissionState.Inactive)
        {
            missionState = MissionState.Active;
            Debug.Log($"Quest '{QuestName}' is now Active.");
        }
    }

    public bool CheckCompletion(PlayerData playerData)
    {
        if (missionState == MissionState.Active)
        {
            foreach (QuestCondition condition in questConditions)
            {
                if (!condition.IsConditionMet(playerData))
                {
                    return false;
                }
            }

            CompleteQuest();
            return true;
        }
        return false;
    }

    private void CompleteQuest()
    {
        if (missionState == MissionState.Active)
        {
            missionState = MissionState.Completed;
            Debug.Log($"Quest '{QuestName}' completed.");
        }
    }

    public void ResetQuest()
    {
        missionState = MissionState.Inactive;
        Debug.Log($"Quest '{QuestName}' has been reset to Inactive.");
    }
}
