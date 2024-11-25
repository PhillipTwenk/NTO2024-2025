using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveData/PLayerSaveData")]
public class PlayerSaveData : ScriptableObject
{
    public List<GameObject> playerBuildings;
    public List<TransformData> buildingsTransform;

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
        Destroy(building);

        int indexBuilding = playerBuildings.IndexOf(building);
        playerBuildings.Remove(building);
        buildingsTransform.Remove(buildingsTransform[indexBuilding]); 
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
