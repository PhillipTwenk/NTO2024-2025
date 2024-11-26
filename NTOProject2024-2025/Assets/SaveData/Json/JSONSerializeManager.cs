using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONSerializeManager : MonoBehaviour
{
    public JSONSerializeManager Instance { get; set; }
    
    
    public List<ScriptableObject> serializableObjects;
    private string savePath;

    private void Awake()
    {
        Instance = this;
    }

    public void AwakeJSONLoad()
    {
        savePath = Application.persistentDataPath;

        // Загружаем данные из JSON при запуске игры
        foreach (var so in serializableObjects)
        {
            if (so is ISerializableSO serializableSO)
            {
                string filePath = Path.Combine(savePath, $"{so.name}.json");
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    serializableSO.DeserializeFromJson(json);
                    Debug.Log($"Загружено {so.name}");
                }
            }
        }
    }

    public void OnApplicationQuit()
    {
        // Сохраняем данные в JSON при выходе из игры
        foreach (var so in serializableObjects)
        {
            if (so is ISerializableSO serializableSO)
            {
                string json = serializableSO.SerializeToJson();
                string filePath = Path.Combine(savePath, $"{so.name}.json");
                File.WriteAllText(filePath, json);
                Debug.Log($"Сохранено {so.name}");
            }
        }
    }
}
