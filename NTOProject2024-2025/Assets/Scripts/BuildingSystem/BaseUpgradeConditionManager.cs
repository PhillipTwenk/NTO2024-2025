using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeConditionManager : MonoBehaviour
{
    public static BaseUpgradeConditionManager Instance {get; set;}

    public static int CurrentBaseLevel;
    public static BuildingData buildingDataMB;

    public static bool IsMinimumOneBuildingOnLevel2;

    public static bool IsMinimumOneMainBuildingOnLevel3;
    public static bool IsMinimumOneOtherBuildingOnLevel3;

    public List<bool> FindNote;
    
    public List<int> NumberOfWorkersForDifferentLevels;

    public static bool EventNatureAtack1Complete;
    public static bool EventNatureAtack2Complete; 

    [TextArea] public string NotEnoughtBuildingsTextError;  
    [TextArea] public string NotEnoughtResourcesTextError;  
    [TextArea] public string NotFoundNoteTextError; 
    [TextArea] public string NotCompleteEventAtackNature;   
    [TextArea] public string NotEnoughtWorkers;
    [TextArea] public string NotEnoughtLevelSomeBuildings;
    [TextArea] public string SuccesUpgradeText;
    
    [TextArea] public string ENDGAME;

    [SerializeField] private GameEvent ResourceMinerRestored;

    [Header("Shield")]
    [SerializeField] private Material ShieldColor;
    [SerializeField] private MeshRenderer ShieldRenderer;
    
    private void Awake()
    {
        Instance = this;
        CurrentBaseLevel = buildingDataMB.Level;
    }

    private void Update()
    {
        //чит
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     CurrentBaseLevel += 1; 
        //     ResourceMinerRestored.TriggerEvent();
        //     Debug.Log(CurrentBaseLevel);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     Dictionary<string, string> testDictionary = new Dictionary<string, string>();
        //     testDictionary.Add("Шкебедедопдодп", "+1488 ");
        //     testDictionary.Add("ДАбулум нипнип", "- 997 deadinside");
        //     APIManager.Instance.CreatePlayerLog("Тестовые логи шкебеде допдоп", UIManagerLocation.WhichPlayerCreate.Name, testDictionary);
        // }
    }

    public List<string> CanUpgradeMobileBase(PlayerResources playerResources)
    {
        int WorkersCount = WorkersInterBuildingControl.Instance.MaxValueOfWorkers;
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        int IronCountPlayer = playerResources.Iron;

        List<string> resultReport = new List<string>();
        bool IsThisReportUnsuccess = false;

        

        switch (CurrentBaseLevel)
        {
            case 1:
                //Перепроверка условий
                if (!FindNote[0])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} 1";
                    resultReport.Add(report);
                }
                if (playerResources.Iron < buildingDataMB.buildingTypeSO.priceUpgrade)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtResourcesTextError}: {playerResources.Iron} / {buildingDataMB.buildingTypeSO.priceUpgrade}";
                    resultReport.Add(report);
                }
                if (WorkersCount < NumberOfWorkersForDifferentLevels[0])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {WorkersCount} / {NumberOfWorkersForDifferentLevels[0]}";
                    resultReport.Add(report);
                }
                



                //Отравка ответа
                if (IsThisReportUnsuccess)
                {
                    return resultReport;
                }
                else
                {
                    resultReport.Clear();
                    resultReport.Add(SuccesUpgradeText);
                    ResourceMinerRestored.TriggerEvent();
                    return resultReport;
                }

                break;
            case 2:
                //Перепроверка условий
                if (!FindNote[1])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} 2";
                    resultReport.Add(report);
                }
                // if (EventNatureAtack1Complete)
                // {
                //     IsThisReportUnsuccess = true;
                //     string report = $"{NotCompleteEventAtackNature} № 1";
                //     resultReport.Add(report);
                // }
                if (playerResources.Iron < buildingDataMB.buildingTypeSO.priceUpgrade)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtResourcesTextError}: {playerResources.Iron} / {buildingDataMB.buildingTypeSO.priceUpgrade}";
                    resultReport.Add(report);
                }
                if (WorkersCount < NumberOfWorkersForDifferentLevels[1])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {WorkersCount} / {NumberOfWorkersForDifferentLevels[1]}";
                    resultReport.Add(report);
                }
                // if (!IsMinimumOneBuildingOnLevel2)
                // {
                //     IsThisReportUnsuccess = true;
                //     string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно здание должно быть 2 уровня";
                //     resultReport.Add(report);
                // }
                
                //Отравка ответа
                if (IsThisReportUnsuccess)
                {
                    return resultReport;
                }
                else
                {
                    resultReport.Clear();
                    resultReport.Add(SuccesUpgradeText);
                    ResourceMinerRestored.TriggerEvent();
                    return resultReport;
                }

                break;
            case 3:
                //Перепроверка условий
                if (!FindNote[2])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} 3";
                    resultReport.Add(report);
                }
                if (playerResources.Iron < buildingDataMB.buildingTypeSO.priceUpgrade)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtResourcesTextError}: {playerResources.Iron} / {buildingDataMB.buildingTypeSO.priceUpgrade}";
                    resultReport.Add(report);
                }
                // if (EventNatureAtack2Complete)
                // {
                //     IsThisReportUnsuccess = true;
                //     string report = $"{NotCompleteEventAtackNature} № 2";
                //     resultReport.Add(report);
                // }
                if (WorkersCount < NumberOfWorkersForDifferentLevels[2])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {WorkersCount} / {NumberOfWorkersForDifferentLevels[2]}";
                    resultReport.Add(report);
                }
                // if (!IsMinimumOneMainBuildingOnLevel3)
                // {
                //     IsThisReportUnsuccess = true;
                //     string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно основное здание должно быть 2 уровня";
                //     resultReport.Add(report);
                // }
                // if (!IsMinimumOneOtherBuildingOnLevel3)
                // {
                //     IsThisReportUnsuccess = true;
                //     string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно дополнительное здание должно быть 2 уровня";
                //     resultReport.Add(report);
                // }


                //Отравка ответа
                if (IsThisReportUnsuccess)
                {
                    return resultReport;
                }
                else
                {
                    resultReport.Clear();
                    resultReport.Add(ENDGAME);
                    ResourceMinerRestored.TriggerEvent();
                    ShieldRenderer.material = ShieldColor;
                    return resultReport;
                }

                break;
        }

        JSONSerializeManager.Instance.OnApplicationQuit();
        
        return null;
    }
}
