using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodItem", menuName = "Inventory/Food Item")]
public class FoodItem : ScriptableObject
{
    [Header("Настройки еды")]
    public string foodName = "Морковка";
    public int hungerRestore = 30;
    public Sprite itemIcon;
    public GameObject foodPrefab; // Префаб для визуализации

    [Header("Описание")]
    [TextArea] public string description;
}
