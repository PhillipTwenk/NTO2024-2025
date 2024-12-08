using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Общее описание свойств сущности
/// </summary>
[CreateAssetMenu(menuName = "ForEntities/Entity")]
public class EntityID : ScriptableObject, ISerializableSO
{
    // Реализация ISerializableSO
    public string SerializeToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void DeserializeFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
    
    
    
    
    
    [TextArea] public string Name;
    
    [Header("Stats")]
    public float speed;
    public float sprintSpeed;
    public float normalSpeed;
    public float speedTurn;
    public int jumpForce;
    
    public string DefaultName;
    public int index;
    public Quest currentQuest;
    public List<Quest> openQuests;

    public void DefaultRevert()
    {
        if (Name != DefaultName)
        {
            string shopName = $"{Name}'sShop";
            APIManager.Instance.DeleteShop(Name, shopName);
            
            APIManager.Instance.DeletePlayer(Name);
        }
        
        Name = DefaultName;
    }
}

