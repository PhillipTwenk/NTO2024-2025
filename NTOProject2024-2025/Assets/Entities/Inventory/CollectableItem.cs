using UnityEngine;

/// <summary>
/// Обработка сбора предметов и добавления их в инвентарь
/// Висит на собираемых объектах на сценах
/// </summary>
public class CollectableItem : MonoBehaviour
{
    public Item item;
    public EntityID playerID;

    /// <summary>
    /// Уничтожает объект и возвращает его SO
    /// </summary>
    public Item CollectItem()
    {
        Destroy(gameObject);
        return item;
    }
    
    /// <summary>
    /// Добавления себя в инвентарь игрока
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerID.playerInventory.AddItem(this);
        }
    }
}