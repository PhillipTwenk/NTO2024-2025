using UnityEngine;

public class BaseUpgradeConditionManager : MonoBehaviour
{
    public static BaseUpgradeConditionManager Instance {get; set;}

    public static int CurrentBaseLevel;

    public static bool IsMinimumOneBuildingOnLevel2;

    public static bool IsMinimumOneMainBuildingOnLevel3;
    public static bool IsMinimumOneOtherBuildingOnLevel3;

    private BuildingData buildingDataMB;

    public static List<bool> FindNote;

    [TextArea]
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


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PLayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();

        buildingDataMB = playerSaveData.BuildingDatas[0];
        CurrentBaseLevel = buildingDataMB.Level;
    }

    private void Update()
    {
        //чит
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CurrentBaseLevel += 1; 
            Debug.Log(CurrentBaseLevel);
        }
    }

    public async List<string> CanUpgradeMobileBase()
    {
        int WorkersCount = WorkersInterBuildingControl.MaxValueOfWorkers;
        int IronCountPlayer = await APIManager.Instance.GetPlayerResources(UIManagerLocation.WhichPlayerCreate.Name).Iron;
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
        int NumberOfWorkers = WorkersInterBuildingControl.Instance.CurrentValueOfWorkers;

        List<string> resultReport;
        bool IsThisReportUnsuccess = false;

        

        switch (CurrentBaseLevel)
        {
            case 1:
                //Перепроверка условий
                if (!FindNote[0])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} № 1";
                    resultReport.Add(report);
                }
                if (playerResources.Iron < buildingDataMB.buildingTypeSO.priceUpgrade)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtResourcesTextError}: {playerResources.Iron} / {buildingDataMB.buildingTypeSO.priceUpgrade}";
                    resultReport.Add(report);
                }
                if (NumberOfWorkers < NumberOfWorkersForDifferentLevels[0])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {NumberOfWorkers} / {NumberOfWorkersForDifferentLevels[0]}";
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
                    return resultReport;
                }

                break;
            case 2:
                //Перепроверка условий
                if (!FindNote[1])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} № 2";
                    resultReport.Add(report);
                }
                if (EventNatureAtack1Complete)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotCompleteEventAtackNature} № 1";
                    resultReport.Add(report);
                }
                if (playerResources.Iron < buildingDataMB.buildingTypeSO.priceUpgrade)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtResourcesTextError}: {playerResources.Iron} / {buildingDataMB.buildingTypeSO.priceUpgrade}";
                    resultReport.Add(report);
                }
                if (NumberOfWorkers < NumberOfWorkersForDifferentLevels[1])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {NumberOfWorkers} / {NumberOfWorkersForDifferentLevels[1]}";
                    resultReport.Add(report);
                }
                if (!IsMinimumOneBuildingOnLevel2)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно здание должно быть 2 уровня";
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
                    return resultReport;
                }

                break;
            case 3:
                //Перепроверка условий
                if (!FindNote[2])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotFoundNoteTextError} № 3";
                    resultReport.Add(report);
                }
                if (EventNatureAtack2Complete)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotCompleteEventAtackNature} № 2";
                    resultReport.Add(report);
                }
                if (NumberOfWorkers < NumberOfWorkersForDifferentLevels[2])
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtWorkers}: {NumberOfWorkers} / {NumberOfWorkersForDifferentLevels[2]}";
                    resultReport.Add(report);
                }
                if (!IsMinimumOneMainBuildingOnLevel3)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно основное здание должно быть 2 уровня";
                    resultReport.Add(report);
                }
                if (!IsMinimumOneOtherBuildingOnLevel3)
                {
                    IsThisReportUnsuccess = true;
                    string report = $"{NotEnoughtLevelSomeBuildings}: хотя бы одно дополнительное здание должно быть 2 уровня";
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
                    return resultReport;
                }
                break;
            default:
        }
    }
}
