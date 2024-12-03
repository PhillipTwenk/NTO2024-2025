 using System;
using System.Collections;
 using System.Threading;
 using System.Threading.Tasks;
using UnityEngine;

public class ResourceMiner : MonoBehaviour
{
    [SerializeField] private int ThisSOIDOB; //6
    public string MinerType;
    [SerializeField] private string IronMinerType;
    [SerializeField] private string CCMinerType;

    [SerializeField] private GameEvent ResourceIronLimitEvent;
    [SerializeField] private GameEvent ResourceCCLimitEvent;
    [SerializeField] private GameEvent UpdateResourcesEvent;

    private bool IsWorkStop;
    private bool OneCycle;
    private bool CanSendMessageToHint;

    [SerializeField] private int TimeProduction;

    private Animator _animator;
    
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); 


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IronMinerPlace"))
        {
            Debug.Log("Добытчик добывает металл");
            MinerType = IronMinerType;
            IsWorkStop = false;
            BuildingData buildingData = GetComponent<BuildingData>();
            if (buildingData.IsThisBuilt)
            {
                OnStartMining();
            }
        }else if (other.gameObject.CompareTag("CCminerPlace"))
        {
            Debug.Log("Добытчик добывает кристалл");
            MinerType = CCMinerType;
            IsWorkStop = false;
            BuildingData buildingData = GetComponent<BuildingData>();
            if (buildingData.IsThisBuilt)
            {
                OnStartMining();
            }
        }
        
    }

    public async void OnStartMining()
    {
        if (!IsWorkStop && !OneCycle)
        {
            OneCycle = true;
            string playerName = UIManagerLocation.WhichPlayerCreate.Name;

            BuildingData buildingData = GetComponent<BuildingData>();
        
            PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();

            if (MinerType == IronMinerType)
            {
                await MinerIronAsync(playerName, playerSaveData, buildingData);
            } //else if (MinerType == CCMinerType)
            // {
            //     await MinerCCAsync(playerName, playerSaveData, buildingData);
            // }
        }
    }

    /// <summary>
    /// Корутина, запускающая процесс добычи железа, пока не превысит лимит по ресурсам
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerResources"></param>
    /// <param name="IronLimit"></param>
    /// <param name="buildingData"></param>
    /// <returns></returns>
    private async Task MinerIronAsync(string playerName, PlayerSaveData playerSaveData, BuildingData buildingData)
    {
        while (gameObject.activeSelf)
        {
            PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
            BuildingSaveData MobileBaseBD = playerSaveData.BuildingDatas[0];

            int StorageAdd = 0;
            foreach (var building in playerSaveData.playerBuildings)
            {
                if (building.transform.GetChild(0).GetComponent<BuildingData>().buildingTypeSO.IDoB == ThisSOIDOB)
                {
                    StorageAdd += building.transform.GetChild(0).GetComponent<BuildingData>().Storage[0];
                }
            }
            int IronLimit = MobileBaseBD.Storage[0] + StorageAdd;

            Debug.Log($"Лимит по металлу: {IronLimit}");

            if ((playerResources.Iron + buildingData.Production[0]) <= IronLimit)
            {
                _animator.SetBool("StopMining",false);
                CanSendMessageToHint = true;
                Debug.Log($"Старое количество металла: {playerResources.Iron}");
                playerResources.Iron += buildingData.Production[0];
                Debug.Log($"Новое количество металла: {playerResources.Iron}");
                await UpdateResources(playerResources, playerName);
                await Task.Delay(TimeProduction);
            } else if ((playerResources.Iron + buildingData.Production[0]) > IronLimit)
            {
                IsWorkStop = true;
                OneCycle = false;
                _animator.SetBool("StopMining",true);
                if (CanSendMessageToHint)
                {
                    ResourceIronLimitEvent.TriggerEvent();
                    CanSendMessageToHint = false;
                }
                break;
            }
        }
    }

    
    /// <summary>
    /// Корутина, запускающая процесс добычи КриоКристаллов, пока не превысит лимит по ресурсам
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerResources"></param>
    /// <param name="CCLimit"></param>
    /// <param name="buildingData"></param>
    /// <returns></returns>
    private async Task MinerCCAsync(string playerName, int CCLimit, BuildingData buildingData)
    {
        while (gameObject.activeSelf)
        {
            PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);

            if ((playerResources.CryoCrystal + buildingData.Production[1]) <= CCLimit)
            {
                Debug.Log($"Старое количество металла: {playerResources.Iron}");
                playerResources.Iron += buildingData.Production[1];
                Debug.Log($"Новое количество металла: {playerResources.Iron}");
                await UpdateResources(playerResources, playerName);
                await Task.Delay(TimeProduction);
            } else if ((playerResources.CryoCrystal + buildingData.Production[1]) > CCLimit)
            {
                IsWorkStop = true;
                OneCycle = false;
                _animator.SetBool("StopMining",true);
                ResourceIronLimitEvent.TriggerEvent();
                break;
            }
        }
    }

    private async Task UpdateResources(PlayerResources playerResources, string playerName)
    {
        await SyncManager.Semaphore.WaitAsync();
        try
        {
            await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
                playerResources.Food, playerResources.CryoCrystal);
            UpdateResourcesEvent.TriggerEvent();
         }
         finally
         {
            SyncManager.Semaphore.Release();
         }
    }

    public void WorkNotStop() => IsWorkStop = false; 
}
