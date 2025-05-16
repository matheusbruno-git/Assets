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

    [Header("Quest Status")]
    public MissionState missionState = MissionState.Inactive;

    [Header("Related NPCs")]
    public NPC_Behaviour[] EmittingNPCs;

    [Header("Quest Conditions")] 
    public ConditionType conditionType;
    public string targetName;
    public int targetAmount;

    public enum ConditionType
    {
        CollectItem,
        TalkToNPC,
        ReachLocation,
        DefeatEnemy
    }

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
            if (IsConditionMet(playerData))
            {
                CompleteQuest();
                return true;
            }
        }
        return false;
    }

    private bool IsConditionMet(PlayerData playerData)
    {
        switch (conditionType)
        {
            case ConditionType.CollectItem:
                return playerData.HasItem(targetName, targetAmount);
            case ConditionType.TalkToNPC:
                return playerData.HasTalkedToNPC(targetName);
            case ConditionType.ReachLocation:
                return playerData.HasReachedLocation(targetName);
            case ConditionType.DefeatEnemy:
                return playerData.HasDefeatedEnemy(targetName, targetAmount);
            default:
                return false;
        }
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
