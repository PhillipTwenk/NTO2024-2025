using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CEResetPlayersData: EditorWindow
{
    private List<PlayerSaveData> playerSaves = new List<PlayerSaveData>();
    private PlayerSaveData DefaultPSD;
    private Vector2 scrollPosition;

    
    [MenuItem("Custom Tools/Reset players data")]
    public static void ShowWindow()
    {
        GetWindow<CEResetPlayersData>("Reset players data window");
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void OnGUI()
    {
        GUILayout.Label("Reset JSON data", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Reset"))
        {
            DeleteJsonFilesFromDirectory();
        }
        
        GUILayout.Space(20);
        
        GUILayout.Label("Reset PlayerSaveData", EditorStyles.boldLabel);

        // Дефолтное значение
        DefaultPSD
            = (PlayerSaveData)EditorGUILayout.ObjectField("Default PSD SO", DefaultPSD, typeof(PlayerSaveData), false, GUILayout.Width(250));
        
        GUILayout.Space(10);

        GUILayout.Label("Add playerSO", EditorStyles.label);
        
        // Кнопка для добавления нового пустого поля
        if (GUILayout.Button("Add field"))
        {
            playerSaves.Add(null);
        }

        GUILayout.Space(5);
        
        if (playerSaves.Count == 0)
        {
            GUILayout.Label("None saved SOs", EditorStyles.label);
        }
        else
        {
            GUILayout.Label("Saved SOs:", EditorStyles.label);
        }
        
        
        // Скроллируемый список полей
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));

        for (int i = 0; i < playerSaves.Count; i++)
        {
            GUILayout.BeginHorizontal();

            // Поле для перетаскивания ScriptableObject
            playerSaves[i] = (PlayerSaveData)EditorGUILayout.ObjectField(
                playerSaves[i], typeof(PlayerSaveData), false, GUILayout.Width(250));

            // Кнопка удаления
            if (GUILayout.Button("Delete", GUILayout.Width(70)))
            {
                playerSaves.RemoveAt(i);
                break;
            }

            GUILayout.EndHorizontal();
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Reset player save datas"))
        {
            for (int i = 0; i < playerSaves.Count; i++)
            {
                playerSaves[i].playerBuildings = DefaultPSD.playerBuildings;
                playerSaves[i].buildingsTransform = DefaultPSD.buildingsTransform;
                playerSaves[i].BuildingDatas = DefaultPSD.BuildingDatas;
                playerSaves[i].BuildingWorkersInformationList = DefaultPSD.BuildingWorkersInformationList;
            }
        }

        GUILayout.EndScrollView();
    }
    
    private void DeleteJsonFilesFromDirectory()
    {
        string folderPath = Application.persistentDataPath;

        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.json");

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                        Debug.Log($"Файл удалён: {file}");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Ошибка при удалении файла {file}: {e.Message}");
                    }
                }
            }
            else
            {
                Debug.Log("Нет JSON файлов для удаления.");
            }
        }
        else
        {
            Debug.LogError("Папка не существует: " + folderPath);
        }
    }

}
