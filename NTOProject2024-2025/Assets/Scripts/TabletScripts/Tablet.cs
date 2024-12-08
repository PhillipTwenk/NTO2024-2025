using TMPro;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private Outline outline;
    public TabletSO TabletInfo;
    public bool isCollected;
    public GameEvent OpenTabletMenuEvent;

    public void OnMouseDown() {
        if(isCollected){
            Debug.Log("Получена заметка");
            isCollected = true;

            //Отмечаем в скрипте контроля уровня базы
            BaseUpgradeConditionManager.Instance.FindNote[int.Parse(TabletInfo.tablet_id)] = true;
            
            UIManager.currentTablet = TabletInfo;
            OpenTabletMenuEvent.TriggerEvent();
        }
    }

    private void OnMouseEnter() {
        outline.enabled = true;
    }
    private void OnMouseExit() {
        outline.enabled = false;
    }
}
