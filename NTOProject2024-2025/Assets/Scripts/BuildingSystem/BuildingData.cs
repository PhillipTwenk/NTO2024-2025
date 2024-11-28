using UnityEngine;
using TMPro;

public class BuildingData : MonoBehaviour
{
    public Building buildingTypeSO;
    public string Title;
    public TextMeshPro textHintBuilding;

    public int Level;
    public int Durability;
    public int Storage;
    public int SaveListIndex;
    
    public TextMeshPro AwaitBuildingThisTMPro;

    [TextArea] public string TextAwaitArriveWorker;
    [TextArea] public string TextAwaitBuildingThis;

    public string AwaitWorkerActionText;
    public string AwaitBuildingActionText;
    
    public void TextPanelBuildingControl(bool IsOpen, string WhichAction)
    {
        if (IsOpen)
        {
            AwaitBuildingThisTMPro.gameObject.SetActive(IsOpen);

            if (WhichAction == AwaitWorkerActionText)
            {
                AwaitBuildingThisTMPro.text = TextAwaitArriveWorker;
            }else if (WhichAction == AwaitBuildingActionText)
            {
                AwaitBuildingThisTMPro.text = TextAwaitBuildingThis;
            }
        }
        else
        {
            AwaitBuildingThisTMPro.gameObject.SetActive(IsOpen);
        }
    }
}
