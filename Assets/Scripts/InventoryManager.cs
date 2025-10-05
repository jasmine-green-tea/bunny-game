using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryInitializer InventoryInitializer;

    [Header("Инвентарь")]
    public Dictionary<InventoryItem, int> inventoryItemsMap = new Dictionary<InventoryItem, int>();


    private void Awake()
    {
        Instance = this;
        
        foreach (InventoryItemBatch entry in InventoryInitializer.items)
        {
            inventoryItemsMap.Add(entry.item, entry.count);
            Debug.Log($"Добавлено в инвентарь: {entry.item.itemName}, количество: {entry.count}");

        }
    }

    // Удалить предмет из инвентаря
    public void RemoveItem(InventoryItem item)
    {
        int currentItemCount = --inventoryItemsMap[item];
        Debug.Log($"Удалено из инвентаря: 1 {item.itemName}, количество: {currentItemCount}");
        if (currentItemCount == 0)
        {
            inventoryItemsMap.Remove(item);
        }
    }

    public Dictionary<InventoryItem, int> GetInventoryItemsMap()
    {
        return inventoryItemsMap;
    }

    // Проверить, есть ли еда в инвентаре
    public bool HasFood()
    {
        return inventoryItemsMap.Count > 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
