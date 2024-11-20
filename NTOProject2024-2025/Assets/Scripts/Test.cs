using UnityEngine;

public class Test : MonoBehaviour
{
   public GameEvent UpdateResources;
   public async void OnMouseDown()
   {
      PlayerResources resources = await APIManager.Instance.GetPlayerResources(UIManagerLocation.WhichPlayerCreate.Name);
      
      await APIManager.Instance.PutPlayerResources(UIManagerLocation.WhichPlayerCreate.Name, resources.Iron + 1, resources.Energy + 1, resources.Food + 1, resources.CryoCrystal + 1);
      
      UpdateResources.TriggerEvent();
   }
}
