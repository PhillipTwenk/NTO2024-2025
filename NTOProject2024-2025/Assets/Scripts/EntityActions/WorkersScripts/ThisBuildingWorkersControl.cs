using UnityEngine;
using TMPro;

public class ThisBuildingWorkersControl : MonoBehaviour
{
    public int CurrentNumberWorkersInThisBuilding;
    public int MaxValueOfWorkersInThisBuilding;
    public int NumberOfActiveWorkersInThisBuilding;

    public TextMeshPro AwaitBuildingThisTMPro;

    [TextArea] public string TextAwaitArriveWorker;
    [TextArea] public string TextAwaitBuildingThis;

    public string AwaitWorkerActionText;
    public string AwaitBuildingActionText;


    public void AddWorkersInThisBuilding(int number, bool IsAdd)
    {
        if (IsAdd)
        {
            if (!(CurrentNumberWorkersInThisBuilding <= MaxValueOfWorkersInThisBuilding))
            {
                CurrentNumberWorkersInThisBuilding += number;
            }
        }else if(!IsAdd){
            if (!(CurrentNumberWorkersInThisBuilding > 0))
            {
                CurrentNumberWorkersInThisBuilding -= number;
            }
        }
    }

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
