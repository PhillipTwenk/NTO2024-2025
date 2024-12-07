using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Предоставление методов для работы с активными квестами
/// Висит на игроке
/// </summary>
public class QuestController : MonoBehaviour
{
    private EntityID playerID;

    //Установление нового активного квеста
    public void ReceiveNewQuest(Quest quest)
    {
        playerID.openQuests.Add(quest);
        quest.active = true;
        playerID.currentQuest = quest;
        quest.OnQuestCompleted += RemoveCompletedQuest;
    }

    //Убрать завершенный квест
    void RemoveCompletedQuest(Quest quest)
    {
        if (playerID.currentQuest == quest)
        {
            playerID.currentQuest = null;
        }

        quest.OnQuestCompleted -= RemoveCompletedQuest;
        playerID.openQuests.Remove(quest);

        if (playerID.openQuests.Count > 0)
        {
            // sets the next quest in the list as the current quest
            playerID.currentQuest = playerID.openQuests[0];
        }
    }

    //При старте игры проверка завершенных квестов и их убирание из массива
    public void QuestInitialize()
    {
        Debug.Log(0000000000000000000000000);
        playerID = UIManagerLocation.WhichPlayerCreate;
        for(int i = playerID.openQuests.Count -1; i>=0; i--)
        {
            if (playerID.openQuests[i].completed)
            {
                RemoveCompletedQuest(playerID.openQuests[i]);
            }
            else
            {
                playerID.openQuests[i].OnQuestCompleted += RemoveCompletedQuest;
            }
        }
    }
    
    void OnDisable()
    {
        foreach (Quest quest in playerID.openQuests)
        {
            quest.OnQuestCompleted -= RemoveCompletedQuest;
        }
    }

}
