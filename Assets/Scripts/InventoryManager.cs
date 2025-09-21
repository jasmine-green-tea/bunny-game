using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("���������")]
    public List<FoodItem> foodItems = new List<FoodItem>();

    [Header("��������� ��������")]
    public FoodItem[] startingItems;

    private void Awake()
    {
        Instance = this;

        // ��������� ��������� ��������
        foreach (FoodItem item in startingItems)
        {
            AddItem(item);
        }
    }

    // �������� ������� � ���������
    public void AddItem(FoodItem item)
    {
        foodItems.Add(item);
        Debug.Log($"��������� � ���������: {item.foodName}");
    }

    // ������� ������� �� ���������
    public void RemoveItem(FoodItem item)
    {
        if (foodItems.Contains(item))
        {
            foodItems.Remove(item);
            Debug.Log($"������� �� ���������: {item.foodName}");
        }
    }

    // �������� ��� �������� ��� ���������
    public List<FoodItem> GetFoodItems()
    {
        return new List<FoodItem>(foodItems);
    }

    // ���������, ���� �� ��� � ���������
    public bool HasFood()
    {
        return foodItems.Count > 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
