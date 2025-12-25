using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using TMPro;

public class ItemButton : PausableObject
{
    [Header("UI элементы")]
    public Image itemIcon;
    public Button button;
    public TMP_Text countText;

    private InventoryItem inventoryItem;
    private Action<InventoryItem> onItemSelected;

    public void Setup(InventoryItem item, int itemCount, Action<InventoryItem> callback)
    {
        inventoryItem = item;
        onItemSelected = callback;
        countText.text = itemCount.ToString();
        countText.gameObject.SetActive(itemCount > 1);

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
