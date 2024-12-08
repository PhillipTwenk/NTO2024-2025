using UnityEngine;

/// <summary>
/// Описание статистических свойств сущности
/// </summary>
[CreateAssetMenu(menuName = "ForEntities/Stats")]
public class Stats : ScriptableObject, ISerializableSO
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
    
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float speedTurn;
    [SerializeField] private int jumpForce;

    // public int DefaultHealth
    // {
    //     get => defaultHealth;
    // }
    // public int Health
    // {
    //     get => health;
    //     set => health = value;
    // }
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    
    public float SprintSpeed
    {
        get => sprintSpeed;
    }
    
    public float NormalSpeed
    {
        get => normalSpeed;
    }
    
    public float SpeedTurn
    {
        get => speedTurn;
    }

    public int JumpForce
    {
        get => jumpForce;
    }

    // public int Damage
    // {
    //     get => damage;
    // }
}