using System.Collections.Generic;
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


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("StopMining",true);
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
                Debug.Log(0912);
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
            } else if (MinerType == CCMinerType)
            {
                 await MinerCCAsync(playerName, playerSaveData, buildingData);
            }
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
        bool isRunning = true;
        while (gameObject.activeSelf && isRunning)
        {
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

            PlayerResources playerResources = await GetResources(playerName);
            if ((playerResources.Iron + buildingData.Production[0]) <= IronLimit)
            {
                _animator.SetBool("StopMining",false);
                CanSendMessageToHint = true;
                Debug.Log($"Старое количество металла: {playerResources.Iron}");
                int OldIronValue = playerResources.Iron;
                playerResources.Iron += buildingData.Production[0];
                Debug.Log($"Новое количество металла: {playerResources.Iron}");
                LogSender(playerName, OldIronValue, playerResources.Iron, IronMinerType);
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
                isRunning = false;
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
    private async Task MinerCCAsync(string playerName, PlayerSaveData playerSaveData, BuildingData buildingData)
    {
        bool isRunning = true;
        while (gameObject.activeSelf && isRunning)
        {
            BuildingSaveData MobileBaseBD = playerSaveData.BuildingDatas[0];

            int StorageAdd = 0;
            foreach (var building in playerSaveData.playerBuildings)
            {
                if (building.transform.GetChild(0).GetComponent<BuildingData>().buildingTypeSO.IDoB == ThisSOIDOB)
                {
                    StorageAdd += building.transform.GetChild(0).GetComponent<BuildingData>().Storage[1];
                }
            }
            int CCLimit = MobileBaseBD.Storage[1] + StorageAdd;

            Debug.Log($"Лимит по КриоКристаллам: {CCLimit}");

            PlayerResources playerResources = await GetResources(playerName);
            if ((playerResources.CryoCrystal + buildingData.Production[1]) <= CCLimit)
            {
                _animator.SetBool("StopMining",false);
                CanSendMessageToHint = true;
                Debug.Log($"Старое количество КриоКристаллов: {playerResources.CryoCrystal}");
                int OldCCValue = playerResources.CryoCrystal;
                playerResources.CryoCrystal += buildingData.Production[1];
                Debug.Log($"Новое количество КриоКристаллов: {playerResources.CryoCrystal}");
                LogSender(playerName, OldCCValue, playerResources.CryoCrystal, CCMinerType);
                await UpdateResources(playerResources, playerName);
                await Task.Delay(TimeProduction);
            } else if ((playerResources.CryoCrystal + buildingData.Production[1]) > CCLimit)
            {
                IsWorkStop = true;
                OneCycle = false;
                _animator.SetBool("StopMining",true);
                if (CanSendMessageToHint)
                {
                    ResourceCCLimitEvent.TriggerEvent();
                    CanSendMessageToHint = false;
                }
                isRunning = false;
            }
        }
    }

    private async Task UpdateResources(PlayerResources playerResources, string playerName)
    {
        await SyncManager.Enqueue(async () =>
        {
            await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
                playerResources.Food, playerResources.CryoCrystal);
            UpdateResourcesEvent.TriggerEvent();
        });
    }

    private async Task<PlayerResources> GetResources(string playerName)
    {
        PlayerResources playerResources = null;
        await SyncManager.Enqueue(async () =>
        {
           playerResources = await APIManager.Instance.GetPlayerResources(playerName);
           Debug.Log(010101010);
        });
        Debug.Log(2020202020);
        return playerResources;
    }

    private void LogSender(string playerName, int OldValue, int NewValue, string Type)
    {
        Dictionary<string,string> playerDictionary = new Dictionary<string, string>();
        if (Type == IronMinerType)
        {
            playerDictionary.Add("IronValueUpdate", $"{NewValue - OldValue}");
            APIManager.Instance.CreatePlayerLog("Добытчик добыл новую партию железа, ресурсы персонажа обновлены",playerName, playerDictionary);
        }else if (Type == CCMinerType)
        {
            playerDictionary.Add("CryoCrystalValueUpdate", $"{NewValue - OldValue}");
            APIManager.Instance.CreatePlayerLog("Добытчик добыл новую партию Криокристаллов, ресурсы персонажа обновлены",playerName, playerDictionary);
        }
    }
    public void WorkNotStop() => IsWorkStop = false; 
}
