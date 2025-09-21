using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class ItemButton : MonoBehaviour
{
    [Header("UI элементы")]
    public Image itemIcon;
    public Text itemNameText;
    public Button button;

    private FoodItem foodItem;
    private Action<FoodItem> onItemSelected;

    public void Setup(FoodItem item, Action<FoodItem> callback)
    {
        foodItem = item;
        onItemSelected = callback;

        // Настраиваем внешний вид кнопки
        if (itemIcon != null && item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

        if (itemNameText != null)
            itemNameText.text = item.foodName;

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        onItemSelected?.Invoke(foodItem);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
