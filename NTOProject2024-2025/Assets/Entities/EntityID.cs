using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Общее описание свойств сущности
/// </summary>
[CreateAssetMenu(menuName = "ForEntities/Entity")]
public class EntityID : ScriptableObject
{
    public Stats playerStats;
    public Inventory playerInventory;
    public Quest currentQuest;
    public List<Quest> openQuests;
}
