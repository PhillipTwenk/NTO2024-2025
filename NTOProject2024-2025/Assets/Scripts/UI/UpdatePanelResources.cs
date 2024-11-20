using System;
using TMPro;
using UnityEngine;

public class UpdatePanelResources : MonoBehaviour
{
    private EntityID ActivePlayer;
    [SerializeField] private TextMeshProUGUI IronTextPanel;
    [SerializeField] private TextMeshProUGUI EnergyTextPanel;
    [SerializeField] private TextMeshProUGUI FoodTextPanel;
    [SerializeField] private TextMeshProUGUI CryoCrystalTextPanel;
    private void Start()
    {
        ActivePlayer = UIManagerLocation.WhichPlayerCreate;
    }

    public async void UpdateResources()
    {
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(ActivePlayer.Name);
        
        IronTextPanel.text = Convert.ToString(playerResources.Iron);
        EnergyTextPanel.text = Convert.ToString(playerResources.Energy);
        FoodTextPanel.text = Convert.ToString(playerResources.Food);
        CryoCrystalTextPanel.text = Convert.ToString(playerResources.CryoCrystal);
        
        Debug.Log("Ресурсы обновлены");
    }

}
