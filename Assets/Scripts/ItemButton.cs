using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class ItemButton : MonoBehaviour
{
    [Header("UI элементы")]
    public Image itemIcon;
    public Button button;

    private InventoryItem inventoryItem;
    private Action<InventoryItem> onItemSelected;

    public void Setup(InventoryItem item, Action<InventoryItem> callback)
    {
        inventoryItem = item;
        onItemSelected = callback;

        // Настраиваем внешний вид кнопки
        if (itemIcon != null && item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        onItemSelected?.Invoke(inventoryItem);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
