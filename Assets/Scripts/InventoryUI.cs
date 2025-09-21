using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("UI элементы")]
    public GameObject inventoryPanel;
    public Transform itemsContainer;
    public GameObject itemButtonPrefab;
    public Text titleText;

    [Header("Настройки")]
    public string feedingTitle = "Выберите еду для кормления";

    private Rabbit targetRabbit;
    private List<GameObject> itemButtons = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    // Показать инвентарь для кормления
    public void ShowFeedingInventory(Rabbit rabbit)
    {
        targetRabbit = rabbit;
        titleText.text = $"{feedingTitle}: {rabbit.rabbitName}";

        // Очищаем предыдущие кнопки
        ClearItemButtons();

        // Создаем кнопки для каждой еды
        foreach (FoodItem foodItem in InventoryManager.Instance.GetFoodItems())
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemsContainer);
            ItemButton itemButton = buttonObj.GetComponent<ItemButton>();

            itemButton.Setup(foodItem, OnFoodSelected);
            itemButtons.Add(buttonObj);
        }

        // Показываем панель
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
        ClearItemButtons();
        targetRabbit = null;
    }

    private void ClearItemButtons()
    {
        foreach (GameObject button in itemButtons)
        {
            Destroy(button);
        }
        itemButtons.Clear();
    }

    private void OnFoodSelected(FoodItem foodItem)
    {
        if (targetRabbit != null && foodItem != null)
        {
            // Кормим кролика
            targetRabbit.Feed(foodItem);
        }

        HideInventory();
    }

    // Закрыть инвентарь по кнопке отмены
    public void OnCancelClicked()
    {
        HideInventory();
    }

    void Update()
    {
        // Скрывать инвентарь при клике вне его
        if (inventoryPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverInventory())
            {
                HideInventory();
            }
        }
    }

    private bool IsPointerOverInventory()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.transform.IsChildOf(inventoryPanel.transform))
                return true;
        }

        return false;
    }
}
