using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Инвентарь")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [Header("Стартовые предметы")]
    public InventoryItem[] startingItems;

    private void Awake()
    {
        Instance = this;

        // Добавляем стартовые предметы
        foreach (InventoryItem item in startingItems)
        {
            AddItem(item);
        }
    }

    // Добавить предмет в инвентарь
    public void AddItem(InventoryItem item)
    {
        inventoryItems.Add(item);
        Debug.Log($"Добавлено в инвентарь: {item.itemName}");
    }

    // Удалить предмет из инвентаря
    public void RemoveItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Debug.Log($"Удалено из инвентаря: {item.itemName}");
        }
    }

    // Получить все предметы для кормления
    public List<InventoryItem> GetInventoryItems()
    {
        return new List<InventoryItem>(inventoryItems);
    }

    // Проверить, есть ли еда в инвентаре
    public bool HasFood()
    {
        return inventoryItems.Count > 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
