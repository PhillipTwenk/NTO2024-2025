using UnityEngine;

/// <summary>
/// Описание свойств инвентаря сущности
/// </summary>
[CreateAssetMenu(menuName = "ForInventory/Inventory")]
public class Inventory : ScriptableObject
{
    public Item[] items;
    public int maxSlots = 10;

    public void Start()
    {
        items = new Item[maxSlots];
    }

    /// <summary>
    /// Добавляет предмет в инвентарь
    /// Вызывается в CollectableItem когда игрок входить в зону действия триггера объекта
    /// </summary>
    /// <param name="collectable"> Ссылка на CollectableItem скрипт предмета </param>
    public void AddItem(CollectableItem collectable)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                // There is space for the item, collect it & destroy the original
                items[i] = collectable.CollectItem();
                return;
            }
        }
        // There is no space, don't collect the item
    }
}