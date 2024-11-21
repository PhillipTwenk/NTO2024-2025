using UnityEngine;

public class TestBuy : MonoBehaviour
{
    [SerializeField] private GameEvent ApiaryShopEvent;
    [SerializeField] private GameEvent HoneyGunShopEvent;
    [SerializeField] private GameEvent MobileBaseShopEvent;
    [SerializeField] private GameEvent StorageShopEvent;
    [SerializeField] private GameEvent ResidentialModuleShopEvent;
    [SerializeField] private GameEvent BreadwinnerShopEvent;
    [SerializeField] private GameEvent PierShopEvent;
    
    public async void OnMouseDown()
    {
        ShopResources res = await APIManager.Instance.GetShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop");
        
        switch (gameObject.name)
        {
            case "ApiaryShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop - 1 , res.HoneyGunShop,res.MobileBaseShop,res.StorageShop,res.ResidentialModuleShop,res.BreadwinnerShop,res.PierShop);
                ApiaryShopEvent.TriggerEvent();
                break;
            case "HoneyGunShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop -1,res.MobileBaseShop,res.StorageShop,res.ResidentialModuleShop,res.BreadwinnerShop,res.PierShop);
                HoneyGunShopEvent.TriggerEvent();
                break;
            case "MobileBaseShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop,res.MobileBaseShop -1,res.StorageShop,res.ResidentialModuleShop,res.BreadwinnerShop,res.PierShop);
                MobileBaseShopEvent.TriggerEvent();
                break;
            case "StorageShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop,res.MobileBaseShop,res.StorageShop -1,res.ResidentialModuleShop,res.BreadwinnerShop,res.PierShop);
                StorageShopEvent.TriggerEvent();
                break;
            case "ResidentialModuleShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop,res.MobileBaseShop,res.StorageShop,res.ResidentialModuleShop -1,res.BreadwinnerShop,res.PierShop);
                ResidentialModuleShopEvent.TriggerEvent();
                break;
            case "BreadwinnerShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop,res.MobileBaseShop,res.StorageShop,res.ResidentialModuleShop,res.BreadwinnerShop -1,res.PierShop);
                BreadwinnerShopEvent.TriggerEvent();
                break;
            case "PierShop":
                await APIManager.Instance.PutShopResources(UIManagerLocation.WhichPlayerCreate.Name, $"{UIManagerLocation.WhichPlayerCreate.Name}'sShop", res.ApiaryShop, res.HoneyGunShop,res.MobileBaseShop,res.StorageShop,res.ResidentialModuleShop,res.BreadwinnerShop,res.PierShop -1);
                PierShopEvent.TriggerEvent();
                break;
        }
    }
}
