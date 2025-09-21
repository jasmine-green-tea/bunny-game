using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Инвентарь")]
    public List<FoodItem> foodItems = new List<FoodItem>();

    [Header("Стартовые предметы")]
    public FoodItem[] startingItems;

    private void Awake()
    {
        Instance = this;

        // Добавляем стартовые предметы
        foreach (FoodItem item in startingItems)
        {
            AddItem(item);
        }
    }

    // Добавить предмет в инвентарь
    public void AddItem(FoodItem item)
    {
        foodItems.Add(item);
        Debug.Log($"Добавлено в инвентарь: {item.foodName}");
    }

    // Удалить предмет из инвентаря
    public void RemoveItem(FoodItem item)
    {
        if (foodItems.Contains(item))
        {
            foodItems.Remove(item);
            Debug.Log($"Удалено из инвентаря: {item.foodName}");
        }
    }

    // Получить все предметы для кормления
    public List<FoodItem> GetFoodItems()
    {
        return new List<FoodItem>(foodItems);
    }

    // Проверить, есть ли еда в инвентаре
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
