using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class ItemButton : MonoBehaviour
{
    [Header("UI ��������")]
    public Image itemIcon;
    public Button button;

    private FoodItem foodItem;
    private Action<FoodItem> onItemSelected;

    public void Setup(FoodItem item, Action<FoodItem> callback)
    {
        foodItem = item;
        onItemSelected = callback;

        // ����������� ������� ��� ������
        if (itemIcon != null && item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

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
