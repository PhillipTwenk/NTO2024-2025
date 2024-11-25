using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveData/PLayerSaveData")]
public class PlayerSaveData : ScriptableObject
{
    public List<GameObject> playerBuildings;
    public List<TransformData> buildingsTransform;
    public List<BuildingSaveData> BuildingDatas;

    /// <summary>
    /// Инициализирует все построеные здания в игре
    /// </summary>
    public void InitializeBuildings()
    {
        if (playerBuildings is not null)
        {
            int i = 0;
            foreach (var building in playerBuildings)
            {
                GameObject newBuilding = Instantiate(building);
                newBuilding.transform.position = buildingsTransform[i].position;
                newBuilding.transform.rotation = buildingsTransform[i].rotation;
                newBuilding.transform.localScale = buildingsTransform[i].scale;

                BuildingData buildingData = newBuilding.GetComponent<BuildingData>();
                buildingData.Level = BuildingDatas[i].Level;
                buildingData.Durability = BuildingDatas[i].Durability;
                buildingData.Storage = BuildingDatas[i].Storage;
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
    }

    /// <summary>
    /// Удаляет конкретное здание
    /// </summary>
    /// <param name="building"> GameObject конкретного здания </param>
    public void DeleteBuilding(GameObject building)
    {
        if (playerBuildings is not null)
        {
            int indexBuilding = building.GetComponent<BuildingData>().SaveListIndex;
            playerBuildings.Remove(building);
            Debug.Log(indexBuilding);
            buildingsTransform.Remove(buildingsTransform[indexBuilding]);
            BuildingDatas.Remove(BuildingDatas[indexBuilding]);

            Destroy(building);
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
