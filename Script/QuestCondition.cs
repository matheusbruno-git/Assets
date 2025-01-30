using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestCondition", menuName = "Scriptables/QuestCondition", order = 1)]
public class QuestCondition : ScriptableObject
{
    public string conditionDescription;
    
    public enum ConditionType
    {
        CollectItem,
        TalkToNPC,
        ReachLocation,
        DefeatEnemy
    }

    public ConditionType conditionType;

    public string targetName;
    public int targetAmount;
    
    public bool IsConditionMet(PlayerData playerData)
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
}
