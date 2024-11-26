using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveData/PLayerSaveData")]
public class PlayerSaveData : ScriptableObject, ISerializableSO
{
    [SerializeField] private string BuildingPrefabsPath;  
    // Реализация ISerializableSO   
    public string SerializeToJson()
    {
        // Сохраняем только данные для сериализации, а не ссылки на объекты
        var serializableData = new SerializableData
        {
            buildingNames = playerBuildings.ConvertAll(b => b.name),
            buildingsTransform = buildingsTransform,
            BuildingDatas = BuildingDatas
        };
        return JsonUtility.ToJson(serializableData, true);
    }

    public void DeserializeFromJson(string json)
    {
        var serializableData = JsonUtility.FromJson<SerializableData>(json);
        
        // Восстанавливаем здания по именам префабов
        playerBuildings = new List<GameObject>();
        foreach (var buildingName in serializableData.buildingNames)
        {
            GameObject prefab = Resources.Load<GameObject>($"Buildings/BuildingPrefabs/{buildingName}");
            if (prefab != null)
            {
                playerBuildings.Add(prefab);
            }
        }

        buildingsTransform = serializableData.buildingsTransform;
        BuildingDatas = serializableData.BuildingDatas;
    }
    
    
    
    
    
    public List<GameObject> playerBuildings;
    public List<TransformData> buildingsTransform;
    public List<BuildingSaveData> BuildingDatas;

    private bool IsDeleteBuidlingProcessActive;

    /// <summary>
    /// Инициализирует все построеные здания в игре
    /// </summary>
    public void InitializeBuildings()
    {
        IsDeleteBuidlingProcessActive = false;
        if (playerBuildings is not null)
        {
            int i = 0;
            foreach (var building in playerBuildings)
            {
                GameObject newBuilding = Instantiate(building);
                newBuilding.transform.position = buildingsTransform[i].position;
                newBuilding.transform.rotation = buildingsTransform[i].rotation;
                newBuilding.transform.localScale = buildingsTransform[i].scale;

                BuildingData buildingData = newBuilding.transform.GetChild(0).GetComponent<BuildingData>();
                buildingData.Level = BuildingDatas[i].Level;
                buildingData.Durability = BuildingDatas[i].Durability;
                buildingData.Storage = BuildingDatas[i].Storage;
                if (i == 0)
                {
                    PlansInShopControl.BaseLevel = buildingData.Level;
                }
                i++;
            }
        }
    }

    /// <summary>
    /// Сбрасывает всю информацию о зданиях
    /// </summary>
    public void RevertBuildingsData()
    {
        playerBuildings.Clear();
        buildingsTransform.Clear();
        BuildingDatas.Clear();
    }

    /// <summary>
    /// Удаляет конкретное здание
    /// </summary>
    /// <param name="building"> GameObject конкретного здания </param>
    public void DeleteBuilding(GameObject building)
    {
        if (playerBuildings is not null && !IsDeleteBuidlingProcessActive)
        {
            IsDeleteBuidlingProcessActive = true;
            BuildingData buildingData = building.GetComponent<BuildingData>();
            int indexBuilding = buildingData.SaveListIndex;
            playerBuildings.Remove(buildingData.buildingTypeSO.PrefabBuilding);
            Debug.Log(indexBuilding);
            buildingsTransform.Remove(buildingsTransform[indexBuilding]);
            BuildingDatas.Remove(BuildingDatas[indexBuilding]);

            Destroy(building.transform.parent.gameObject);

            IsDeleteBuidlingProcessActive = false;
        }
    }
    
}

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }
}

[System.Serializable]
public class BuildingSaveData
{
    public int Level;
    public int Durability;
    public int Storage;
    public int SaveListIndex;

    public BuildingSaveData(BuildingData buildingData)
    {
        Level = buildingData.Level;
        Durability = buildingData.Durability;
        Storage = buildingData.Storage;
        SaveListIndex = buildingData.SaveListIndex;
    }
}

[System.Serializable]
public class SerializableData
{
    public List<string> buildingNames;
    public List<TransformData> buildingsTransform;
    public List<BuildingSaveData> BuildingDatas;
}
