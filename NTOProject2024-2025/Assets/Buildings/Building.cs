using UnityEngine;

[CreateAssetMenu(menuName = "Building")]
public class Building : ScriptableObject
{
    [SerializeField] private int level; // Уровень
    [SerializeField] private int levelDefault; // Начальный уровень
    [SerializeField] private int durability; // Прочность
    [SerializeField] private int durabilityDefault; // Начальная прочность
    [SerializeField] private int energyHoneyConsumption; // Потребление энергомеда
    [SerializeField] private int energyHoneyConsumptionDefault; // Начальное Потребление энергомеда
    [SerializeField] private int resourceProduction; // Производство собственного ресурса
    [SerializeField] private int resourceProductionDefault; // Начальное Производство собственного ресурса
    [SerializeField] private bool isActive; // Активно ли данное здание ( Можно менять в меню подробного просмотра )
    [SerializeField] private bool isBuilt; // Построено ли данное здание
    [SerializeField] private Transform depositPosition; // Местоположение данного здания
    [SerializeField] private int storage; // Локальное хранилище здания
    [SerializeField] private int storageDefault; // начальное Локальное хранилище здания
    [SerializeField] private int storageLimit; // Лимит хранилища данного здания
    [SerializeField] private GameObject prefabBuilding; // Префаб строения
    [SerializeField] private GameObject prefabBeforeBuilding; // Префаб триггера перед поставновкой здания
    [SerializeField] private int OwnIndex; //Какое по счету здание, среди зданий такого эе типа, начинается с 0

    public void DefaultRevert()
    {
        level = levelDefault;
        durability = durabilityDefault;
        energyHoneyConsumption = energyHoneyConsumptionDefault;
        resourceProduction = resourceProductionDefault;
        isActive = false;
        isBuilt = false;
        depositPosition = null;
        storage = storageDefault;
    }
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
    
    public GameObject PrefabBeforeBuilding
    {
        get => prefabBeforeBuilding;
    }
}
