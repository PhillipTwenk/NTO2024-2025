using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Общее описание свойств сущности
/// </summary>
[CreateAssetMenu(menuName = "ForEntities/Entity")]
public class EntityID : ScriptableObject
{
    [TextArea] public string Name;
    public string DefaultName;
    
    
    public Stats playerStats;
    public Quest currentQuest;

    public bool IsThisCharacterLoadInThisGame;

    public List<Quest> openQuests;

    public void DefaultRevert()
    {
        Name = DefaultName;
        IsThisCharacterLoadInThisGame = false;
    }
}

