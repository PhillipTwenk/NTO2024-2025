using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceMiner : MonoBehaviour
{
    public string MinerType;
    [SerializeField] private string IronMinerType;
    [SerializeField] private string CCMinerType;

    [SerializeField] private GameEvent ResourceIronLimitEvent;
    [SerializeField] private GameEvent ResourceCCLimitEvent;
    [SerializeField] private GameEvent UpdateResourcesEvent;

    private bool IsWorkStop;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IronMinerPlace"))
        {
            MinerType = IronMinerType;
            IsWorkStop = false;
        }else if (other.gameObject.CompareTag("CCminerPlace"))
        {
            MinerType = CCMinerType;
            IsWorkStop = false;
        }
    }

    public async void OnEnable()
    {

        if (!IsWorkStop)
        {
            string playerName = UIManagerLocation.WhichPlayerCreate.Name;

            PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
        
            BuildingData buildingData = GetComponent<BuildingData>();
        
            PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
            BuildingSaveData MobileBaseBD = playerSaveData.BuildingDatas[0];

            int IronLimit = MobileBaseBD.Storage[0];
            int CryoCrystalLimit = MobileBaseBD.Storage[2];

            if (MinerType == IronMinerType)
            {
                await MinerIronAsync(playerName, playerResources, IronLimit, buildingData);
            }else if (MinerType == CCMinerType)
            {
                await MinerCCAsync(playerName, playerResources, CryoCrystalLimit, buildingData);
            }
        }
    }

    private void OnDisable()
    {
        
    }

    /// <summary>
    /// Корутина, запускающая процесс добычи железа, пока не превысит лимит по ресурсам
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerResources"></param>
    /// <param name="IronLimit"></param>
    /// <param name="buildingData"></param>
    /// <returns></returns>
    private async Task MinerIronAsync(string playerName, PlayerResources playerResources, int IronLimit, BuildingData buildingData)
    {
        while ((playerResources.Iron + buildingData.Production[0]) <= IronLimit)
        {
            playerResources.Iron += buildingData.Production[0];
            await UpdateResources(playerResources, playerName);
            await Task.Delay(15000); // Эквивалент 15 секунд
        }

        if ((playerResources.Iron + buildingData.Production[0]) >= IronLimit)
        {
            IsWorkStop = true;
            ResourceIronLimitEvent.TriggerEvent();
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
    private async Task MinerCCAsync(string playerName, PlayerResources playerResources, int CCLimit, BuildingData buildingData)
    {
        while ((playerResources.CryoCrystal + buildingData.Production[2]) <= CCLimit)
        {
            playerResources.CryoCrystal += buildingData.Production[2];
            await UpdateResources(playerResources, playerName);
            await Task.Delay(15000); // Эквивалент 15 секунд
        }
        if ((playerResources.CryoCrystal + buildingData.Production[2]) >= CCLimit)
        {
            ResourceCCLimitEvent.TriggerEvent();
        }
    }

    private async Task UpdateResources(PlayerResources playerResources, string playerName)
    {
        await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
            playerResources.Food, playerResources.CryoCrystal);
        UpdateResourcesEvent.TriggerEvent();
    }
}
