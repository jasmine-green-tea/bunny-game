using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("���������")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [Header("��������� ��������")]
    public InventoryItem[] startingItems;

    private void Awake()
    {
        Instance = this;

        // ��������� ��������� ��������
        foreach (InventoryItem item in startingItems)
        {
            AddItem(item);
        }
    }

    // �������� ������� � ���������
    public void AddItem(InventoryItem item)
    {
        inventoryItems.Add(item);
        Debug.Log($"��������� � ���������: {item.itemName}");
    }

    // ������� ������� �� ���������
    public void RemoveItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Debug.Log($"������� �� ���������: {item.itemName}");
        }
    }

    // �������� ��� �������� ��� ���������
    public List<InventoryItem> GetInventoryItems()
    {
        return new List<InventoryItem>(inventoryItems);
    }

    // ���������, ���� �� ��� � ���������
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
