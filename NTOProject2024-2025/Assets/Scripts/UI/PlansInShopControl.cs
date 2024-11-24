using System;
using UnityEngine;
using UnityEngine.UI;

public class PlansInShopControl : MonoBehaviour
{
    public static int BaseLevel;
    
    [SerializeField] private GameObject PanelHoneyGun;
    [SerializeField] private GameObject PanelStorage;
    [SerializeField] private GameObject PanelPier;
    
    [SerializeField] private GameObject PanelHoneyGunBought;
    [SerializeField] private GameObject PanelStorageBought;
    [SerializeField] private GameObject PanelPierBought;
    [SerializeField] private GameObject NotEnoughtResourcesTextPanel;

    [SerializeField] private Button _buttonHG;
    [SerializeField] private Button _buttonS;
    [SerializeField] private Button _buttonP;

    public bool IsHoneyGunBought;
    public bool IsStorageBought;
    public bool IsPierBought;

    [SerializeField] private int PriceIronValue;
    [SerializeField] private int PriceCryoCrystalValue;

    [SerializeField] private string HoneyGunName;
    [SerializeField] private string StorageName;
    [SerializeField] private string PierName;

    private string WhichPanelActive;


    private void Awake()
    {
        BaseLevel = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BaseLevel += 1;
            Debug.Log(BaseLevel);
        }
    }
    /// <summary>
    /// При включении панели бартера в зависимости от уровня базы дает разные предложения
    /// </summary>
    private void OnEnable()
    {
        NotEnoughtResourcesTextPanel.SetActive(false);
        PanelHoneyGunBought.SetActive(false);
        PanelStorageBought.SetActive(false);
        PanelPierBought.SetActive(false);
        switch (BaseLevel)
        {
            case 1:
                PanelHoneyGun.SetActive(true);
                WhichPanelActive = HoneyGunName;
                if (IsHoneyGunBought)
                {
                    PanelHoneyGunBought.SetActive(true);
                    _buttonHG.enabled = false;
                }
                break;
            case 2:
                PanelStorage.SetActive(true);
                WhichPanelActive = StorageName;
                if (IsStorageBought)
                {
                    PanelStorageBought.SetActive(true);
                    _buttonS.enabled = false;
                }
                break;
            case 3:
                PanelPier.SetActive(true);
                WhichPanelActive = PierName;
                if (IsPierBought)
                {
                    PanelPierBought.SetActive(true);
                    _buttonP.enabled = false;
                }
                break;
        }
    }

    /// <summary>
    /// Нажатие на кнопку покупки чертежа
    /// </summary>
    public async void ClickBuyPlanButton()
    {
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
        
        int playerIron = playerResources.Iron;
        int playerCryoCrystal = playerResources.CryoCrystal;
        int playerEnergy = playerResources.Energy;
        int playerFood = playerResources.Food;

        if (playerIron >= PriceIronValue && playerCryoCrystal >= PriceCryoCrystalValue)
        {
            NotEnoughtResourcesTextPanel.SetActive(false);
            
            await APIManager.Instance.PutPlayerResources(playerName, playerIron - PriceIronValue, playerEnergy, playerFood, playerCryoCrystal-PriceCryoCrystalValue);

            string shopName = $"{playerName}'sShop";
            ShopResources shopResources = await APIManager.Instance.GetShopResources(playerName, shopName);
                
            if (WhichPanelActive == HoneyGunName && !IsHoneyGunBought)
            {
                shopResources.HoneyGunShop = 0;
                await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.ApiaryShop, shopResources.HoneyGunShop, shopResources.MobileBaseShop, shopResources.StorageShop, shopResources.ResidentialModuleShop, shopResources.BreadwinnerShop, shopResources.PierShop);
                IsHoneyGunBought = true;
                PanelHoneyGunBought.SetActive(true);
                _buttonHG.enabled = false;
            }
            if (WhichPanelActive == StorageName && !IsStorageBought)
            {
                shopResources.StorageShop = 0;
                await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.ApiaryShop, shopResources.HoneyGunShop, shopResources.MobileBaseShop, shopResources.StorageShop, shopResources.ResidentialModuleShop, shopResources.BreadwinnerShop, shopResources.PierShop);
                IsStorageBought = true;
                PanelStorageBought.SetActive(true);
                _buttonS.enabled = false;
            }
            if (WhichPanelActive == PierName && !IsPierBought)
            {
                shopResources.PierShop = 0;
                await APIManager.Instance.PutShopResources(playerName, shopName, shopResources.ApiaryShop, shopResources.HoneyGunShop, shopResources.MobileBaseShop, shopResources.StorageShop, shopResources.ResidentialModuleShop, shopResources.BreadwinnerShop, shopResources.PierShop);
                IsPierBought = true;
                PanelPierBought.SetActive(true);
                _buttonP.enabled = false;
            }
        }
        else
        {
            NotEnoughtResourcesTextPanel.SetActive(true);
        }
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
}
