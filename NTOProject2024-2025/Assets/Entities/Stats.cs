using UnityEngine;

/// <summary>
/// Описание статистических свойств сущности
/// </summary>
[CreateAssetMenu(menuName = "ForEntities/Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] private int defaultHealth;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int damage;

    public int DefaultHealth
    {
        get => defaultHealth;
    }
    public int Health
    {
        get => health;
        set => health = value;
    }
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public int JumpForce
    {
        get => jumpForce;
    }

    public int Damage
    {
        get => damage;
    }
}