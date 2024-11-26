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

    private void OnMouseDown() {
        OpenTabletMenuEvent.TriggerEvent();
        isCollected = true;
    }
}
