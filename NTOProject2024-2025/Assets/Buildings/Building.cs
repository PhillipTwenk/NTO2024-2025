using UnityEngine;

[CreateAssetMenu(menuName = "Building")]
public class Building : ScriptableObject
{
    [SerializeField] private int level;
    [SerializeField] private int durability;
    [SerializeField] private int energyHoneyConsumption;
    [SerializeField] private int resourceProduction;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isBuilt;
    [SerializeField] private Transform depositPosition;
    [SerializeField] private int storage;
    [SerializeField] private int storageLimit;
    [SerializeField] private GameObject prefabBuilding;

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int Durability
    {
        get => durability;
        set => durability = value;
    }
    
    public int EnergyHoneyConsumption
    {
        get => energyHoneyConsumption;
        set => energyHoneyConsumption = value;
    }
    
    public int ResourceProduction
    {
        get => resourceProduction;
        set => resourceProduction = value;
    }

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }
    
    public bool IsBuilt
    {
        get => isBuilt;
        set => isBuilt = value;
    }
    
    public Transform DepositPosition
    {
        get => depositPosition;
        set => depositPosition = value;
    }

    public int Storage
    {
        get => storage;
        set => storage = value;
    }

    public int StorageLimit
    {
        get => storageLimit;
        set => storageLimit = value;
    }

    public GameObject PrefabBuilding
    {
        get => prefabBuilding;
    }
}
