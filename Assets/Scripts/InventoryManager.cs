using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryInitializer InventoryInitializer;

    [Header("���������")]
    public Dictionary<InventoryItem, int> inventoryItemsMap = new Dictionary<InventoryItem, int>();


    private void Awake()
    {
        Instance = this;
        
        foreach (InventoryItemBatch entry in InventoryInitializer.items)
        {
            inventoryItemsMap.Add(entry.item, entry.count);
            Debug.Log($"��������� � ���������: {entry.item.itemName}, ����������: {entry.count}");

        }
    }

    // ������� ������� �� ���������
    public void RemoveItem(InventoryItem item)
    {
        int currentItemCount = --inventoryItemsMap[item];
        Debug.Log($"������� �� ���������: 1 {item.itemName}, ����������: {currentItemCount}");
        if (currentItemCount == 0)
        {
            inventoryItemsMap.Remove(item);
        }
    }

    public Dictionary<InventoryItem, int> GetInventoryItemsMap()
    {
        return inventoryItemsMap;
    }

    // ���������, ���� �� ��� � ���������
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
