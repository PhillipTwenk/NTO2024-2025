using TMPro;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    public TabletSO TabletInfo;
    public bool isCollected;
    public GameEvent OpenTabletMenuEvent;


    public void OnMouseDown() {
        if(isCollected){
            Debug.Log("Получена заметка");
            isCollected = true;

            //Отмечаем в скрипте контроля уровня базы
            BaseUpgradeConditionManager.FindNote[int.Parse(TabletInfo.tablet_id)] = true;

            UIManager.currentTablet = TabletInfo;
            OpenTabletMenuEvent.TriggerEvent();
        }
    }
}
