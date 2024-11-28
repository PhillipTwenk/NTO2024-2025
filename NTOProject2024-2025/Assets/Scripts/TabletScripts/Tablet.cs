using UnityEngine;

public class Tablet : MonoBehaviour
{
    public TabletSO TabletInfo;
    public bool isCollected;
    public GameEvent OpenTabletMenuEvent;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnMouseDown() {
        if(isCollected){
            Debug.Log(1);
            isCollected = true;
            UIManager.currentTablet = TabletInfo;
            OpenTabletMenuEvent.TriggerEvent();
        }
    }
}
