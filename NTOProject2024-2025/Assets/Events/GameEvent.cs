using System.Collections.Generic;  
using UnityEngine;  
  
/// <summary>
/// SO для создания ивентов, а также предоставление методов работы с ними
/// </summary>
[CreateAssetMenu(menuName ="Game Event")]  
public class GameEvent : ScriptableObject
{
    [TextArea]
    [SerializeField] private string Description;
    
    //Список скриптов-слушателей  
    private List<GameEventListener> listeners = new List<GameEventListener>();  
  
    //Метод, дергающий всех слушателей и выполняющий все их методы  
    public void TriggerEvent()  
    {        
        for (int i = listeners.Count -1; i >= 0; i--)  
        {            
            listeners[i].OnEventTriggered();  
        }    
        
    }  
    
    
    //Добавление слушателей в массив  
    public void AddListener(GameEventListener listener)  
    {        
        listeners.Add(listener);  
    }  
    
    
    //Удаление слушателей из массива  
    public void RemoveListener(GameEventListener listener)  
    {  
        listeners.Remove(listener);  
    }
}