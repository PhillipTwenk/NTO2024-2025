using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Слушает ивенты со всех сцен
/// В инспекторе в переменную UnityEvent загружается метод нужного скрипта, который будет вызван при прослушке данного ивента
/// Висит на каждом объекте, которому нужно прослушать нужный ивент
/// Для каждого отдельного ивента на объекте создается новый компонент этого скрипта
/// </summary>
public class GameEventListener : MonoBehaviour
{
    //[TextArea]
    //[SerializeField] private string Description;
    
    //Сюда загружается нужный SO-экземпляр ивента
    public GameEvent gameEvent;

    //Здесь из инспектора загружаются все необходимые методы
    public UnityEvent onEventTriggered;

    //Методы добавляют/удаляют данного слушателя из массива ивент-экземпляра
    void OnEnable()
    {
        gameEvent.AddListener(this);
    }
    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    
    //Метод запускает все нужные методы из onEventTriggered
    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();     
    }
}
