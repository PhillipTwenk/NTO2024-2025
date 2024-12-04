using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlansInShopControl : MonoBehaviour
{
    [SerializeField] private GameObject PanelHoneyGun;
    [SerializeField] private GameObject PanelStorage;
    [SerializeField] private GameObject PanelPier;
    
    [SerializeField] private GameObject PanelHoneyGunBought;
    [SerializeField] private GameObject PanelStorageBought;
    [SerializeField] private GameObject PanelPierBought;
    [SerializeField] private GameObject NotEnoughtResourcesTextPanel;

    [SerializeField] private GameEvent UpdateResourcesEvent;

    [SerializeField] private Button _buttonHG;
    [SerializeField] private Button _buttonS;
    [SerializeField] private Button _buttonP;
    
    [SerializeField] private string HoneyGunName;
    [SerializeField] private string StorageName;
    [SerializeField] private string PierName;

    [SerializeField] private Plan HGPlan;
    [SerializeField] private Plan SPlan;
    [SerializeField] private Plan PPlan;
    

    private string WhichPanelActive;

    /// <summary>
    /// При включении панели бартера в зависимости от уровня базы дает разные предложения
    /// </summary>
    private async void OnEnable()
    {
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);
        NotEnoughtResourcesTextPanel.SetActive(false);
        PanelHoneyGunBought.SetActive(false);
        PanelStorageBought.SetActive(false);
        PanelPierBought.SetActive(false);
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        string shopName = $"{playerName}'sShop";
        ShopResources shopResources = await APIManager.Instance.GetShopResources(playerName, shopName);
        switch (BaseUpgradeConditionManager.CurrentBaseLevel)
        {
            case 1:
                PanelHoneyGun.SetActive(true);
                WhichPanelActive = HoneyGunName;
                if (shopResources.HoneyGun.IsPurchased)
                {
                    PanelHoneyGunBought.SetActive(true);
                    _buttonHG.enabled = false;
                }
                break;
            case 2:
                PanelStorage.SetActive(true);
                WhichPanelActive = StorageName;
                if (shopResources.Storage.IsPurchased)
                {
                    PanelStorageBought.SetActive(true);
                    _buttonS.enabled = false;
                }
                break;
            case 3:
                PanelPier.SetActive(true);
                WhichPanelActive = PierName;
                if (shopResources.Pier.IsPurchased)
                {
                    PanelPierBought.SetActive(true);
                    _buttonP.enabled = false;
                }
                break;
        }
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }

    /// <summary>
    /// Нажатие на кнопку покупки чертежа
    /// </summary>
    public async void ClickBuyPlanButton()
    {
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);

        string shopName = $"{playerName}'sShop";
        ShopResources shopResources = await APIManager.Instance.GetShopResources(playerName, shopName);

        int playerIron = playerResources.Iron;
        int playerCryoCrystal = playerResources.CryoCrystal;
        int playerEnergy = playerResources.Energy;
        int playerFood = playerResources.Food;

        NotEnoughtResourcesTextPanel.SetActive(false);

        Dictionary<string,string> shopDictionary = new Dictionary<string, string>();
        Dictionary<string,string> playerResourcesDictionary = new Dictionary<string, string>();
        
        if (WhichPanelActive == HoneyGunName)
        {
            if (!shopResources.HoneyGun.IsPurchased)
            {
                if (playerIron >= shopResources.HoneyGun.IronPrice &&
                    playerCryoCrystal >= shopResources.HoneyGun.CryoCrystalPrice)
                {
                    shopResources.HoneyGun.IsPurchased = true;
                    
                    shopDictionary.Add("HoneyGunShopValueUpdate", "true");
                    APIManager.Instance.CreateShopLog("Куплен чертеж медовой пушки (товары в единичном экземпляре)", playerName, shopName, shopDictionary);
                    
                    playerResourcesDictionary.Add("IronValueUpdate", $"{(playerIron - shopResources.HoneyGun.IronPrice) - playerIron}");
                    playerResourcesDictionary.Add("CrytoCrystalValueUpdate", $"{(playerCryoCrystal - shopResources.HoneyGun.CryoCrystalPrice) - playerCryoCrystal}");
                    APIManager.Instance.CreatePlayerLog("Куплен чертеж медовой пушки в магазине, потрачены металл и кристаллы", playerName, playerResourcesDictionary);
                    await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.Apiary,
                        shopResources.HoneyGun, shopResources.MobileBase, shopResources.Storage,
                        shopResources.ResidentialModule, shopResources.Minner, shopResources.Pier);
                    await APIManager.Instance.PutPlayerResources(playerName, playerIron - shopResources.HoneyGun.IronPrice, playerEnergy, playerFood, playerCryoCrystal - shopResources.HoneyGun.CryoCrystalPrice);
                    PanelHoneyGunBought.SetActive(true);
                    _buttonHG.enabled = false;
                    UIManager.Instance.AddNewPlanInPanel(HGPlan);
                }
                else
                {
                    NotEnoughtResourcesTextPanel.SetActive(true);
                }
            }
            else
            {
                PanelHoneyGunBought.SetActive(true);
                _buttonHG.enabled = false;
            }
        }

        if (WhichPanelActive == StorageName)
        {
            if (!shopResources.Storage.IsPurchased)
            {
                if (playerIron >= shopResources.Storage.IronPrice &&
                    playerCryoCrystal >= shopResources.Storage.CryoCrystalPrice)
                {
                    shopResources.Storage.IsPurchased = true;
                    
                    shopDictionary.Add("StorageShopValueUpdate", "true");
                    APIManager.Instance.CreateShopLog("Куплен чертеж хранилища ресурсов (товары в единичном экземпляре)", playerName, shopName, shopDictionary);
                    
                    playerResourcesDictionary.Add("IronValueUpdate", $"{(playerIron - shopResources.Storage.IronPrice) - playerIron}");
                    playerResourcesDictionary.Add("CrytoCrystalValueUpdate", $"{(playerCryoCrystal - shopResources.Storage.CryoCrystalPrice) - playerCryoCrystal}");
                    APIManager.Instance.CreatePlayerLog("Куплен чертеж хранилища в магазине, потрачены металл и кристаллы", playerName, playerResourcesDictionary);
                    
                    await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.Apiary,
                        shopResources.HoneyGun, shopResources.MobileBase, shopResources.Storage,
                        shopResources.ResidentialModule, shopResources.Minner, shopResources.Pier);
                    await APIManager.Instance.PutPlayerResources(playerName, playerIron - shopResources.Storage.IronPrice, playerEnergy, playerFood, playerCryoCrystal - shopResources.Storage.CryoCrystalPrice);
                    PanelStorageBought.SetActive(true);
                    _buttonS.enabled = false;
                    UIManager.Instance.AddNewPlanInPanel(SPlan);
                }
                else
                {
                    NotEnoughtResourcesTextPanel.SetActive(true);
                }
            }
            else
            {
                PanelHoneyGunBought.SetActive(true);
                _buttonHG.enabled = false;
            }
        }

        if (WhichPanelActive == PierName)
        {
            if (!shopResources.Pier.IsPurchased)
            {
                if (playerIron >= shopResources.Pier.IronPrice &&
                    playerCryoCrystal >= shopResources.Pier.CryoCrystalPrice)
                {
                    shopResources.Pier.IsPurchased = true;
                    
                    shopDictionary.Add("PierShopValueUpdate", "true");
                    APIManager.Instance.CreateShopLog("Куплен чертеж пристани (товары в единичном экземпляре)", playerName, shopName, shopDictionary);
                    
                    playerResourcesDictionary.Add("IronValueUpdate", $"{(playerIron - shopResources.Pier.IronPrice) - playerIron}");
                    playerResourcesDictionary.Add("CrytoCrystalValueUpdate", $"{(playerCryoCrystal - shopResources.Pier.CryoCrystalPrice) - playerCryoCrystal}");
                    APIManager.Instance.CreatePlayerLog("Куплен чертеж пристани в магазине, потрачены металл и кристаллы", playerName, playerResourcesDictionary);
                    
                    await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.Apiary,
                        shopResources.HoneyGun, shopResources.MobileBase, shopResources.Storage,
                        shopResources.ResidentialModule, shopResources.Minner, shopResources.Pier);
                    await APIManager.Instance.PutPlayerResources(playerName, playerIron - shopResources.Pier.IronPrice, playerEnergy, playerFood, playerCryoCrystal - shopResources.Pier.CryoCrystalPrice);
                    PanelPierBought.SetActive(true);
                    _buttonP.enabled = false;
                    UIManager.Instance.AddNewPlanInPanel(PPlan);
                }
                else
                {
                    NotEnoughtResourcesTextPanel.SetActive(true);
                }
            }
            else
            {
                PanelHoneyGunBought.SetActive(true);
                _buttonHG.enabled = false;
            }
        }

        UpdateResourcesEvent.TriggerEvent();

        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
}

