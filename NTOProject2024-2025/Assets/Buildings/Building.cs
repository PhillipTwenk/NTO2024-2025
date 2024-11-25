using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building")]
public class Building : ScriptableObject
{
    [SerializeField] private List<int> levelListLevel;
    
    [SerializeField] private List<int> durabilityListLevel;
    
    [SerializeField] private List<int> energyHoneyConsumptionListLevel;

    [SerializeField] private List<int> resourceProductionListLevel;
    
    [SerializeField] private List<int> storageListLevel;
    
    public int priceBuilding; // Стоимость здания
    
    [SerializeField] private GameObject prefabBuilding; // Префаб строения
    [SerializeField] private GameObject prefabBeforeBuilding; // Префаб триггера перед поставновкой здания

    // public void DefaultRevert()
    // {
    //     level = levelDefault;
    //     durability = durabilityDefault;
    //     energyHoneyConsumption = energyHoneyConsumptionDefault;
    //     resourceProduction = resourceProductionDefault;
    //     isActive = false;
    //     isBuilt = false;
    //     depositPosition = null;
    //     storage = storageDefault;
    // }
    // public int Level
    // {
    //     get => level;
    //     set => level = value;
    // }

    /// <summary>
    /// Возвращает значение уровня
    /// </summary>
    /// <param name="levelMobileBase"></param>
    /// <returns></returns>
    public int Level(int levelMobileBase)
    {
        switch (levelMobileBase)
        {
            case 1:
                return levelListLevel[0];
                break;
            case 2:
                return levelListLevel[1];
                break;
            case 3:
                return levelListLevel[2];
                break;
            default: return levelListLevel[0];
        }
    }
    
    /// <summary>
    /// Возвращает значение лимита локальных ресурсов в зависимости от уровня базы
    /// </summary>
    /// <param name="levelMobileBase"></param>
    /// <returns></returns>
    public int StorageLimit(int levelMobileBase)
    {
        switch (levelMobileBase)
        {
            case 1:
                return storageListLevel[0];
                break;
            case 2:
                return storageListLevel[1];
                break;
            case 3:
                return storageListLevel[2];
                break;
            default: return storageListLevel[0];
        }
    }
    
    /// <summary>
    /// Возвращает значение медопотребления в зависимости от уровня базы
    /// </summary>
    /// <param name="levelMobileBase"></param>
    /// <returns></returns>
    public int EnergyHoneyConsumpiton(int levelMobileBase)
    {
        switch (levelMobileBase)
        {
            case 1:
                return energyHoneyConsumptionListLevel[0];
                break;
            case 2:
                return energyHoneyConsumptionListLevel[1];
                break;
            case 3:
                return energyHoneyConsumptionListLevel[2];
                break;
            default: return energyHoneyConsumptionListLevel[0];
        }
    }
    
    /// <summary>
    /// Возвращает значение Прочности в зависимости от уровня базы
    /// </summary>
    /// <param name="levelMobileBase"></param>
    /// <returns></returns>
    public int Durability(int levelMobileBase)
    {
        switch (levelMobileBase)
        {
            case 1:
                return durabilityListLevel[0];
                break;
            case 2:
                return durabilityListLevel[1];
                break;
            case 3:
                return durabilityListLevel[2];
                break;
            default: return durabilityListLevel[0];
        }
    }
    
    /// <summary>
    /// Возвращает значение производства в зависимости от уровня базы
    /// </summary>
    /// <param name="levelMobileBase"></param>
    /// <returns></returns>
    public int Production(int levelMobileBase)
    {
        switch (levelMobileBase)
        {
            case 1:
                return resourceProductionListLevel[0];
                break;
            case 2:
                return resourceProductionListLevel[1];
                break;
            case 3:
                return resourceProductionListLevel[2];
                break;
            default: return resourceProductionListLevel[0];
        }
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
