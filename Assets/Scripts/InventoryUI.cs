using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("UI ��������")]
    public GameObject inventoryPanel;
    public Transform itemsContainer;
    public GameObject itemButtonPrefab;
    public Text titleText;

    [Header("���������")]
    public string feedingTitle = "�������� ��� ��� ���������";

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

    // �������� ��������� ��� ���������
    public void ShowFeedingInventory(Rabbit rabbit)
    {
        targetRabbit = rabbit;
        titleText.text = $"{feedingTitle}: {rabbit.rabbitName}";

        // ������� ���������� ������
        ClearItemButtons();

        // ������� ������ ��� ������ ���
        foreach (FoodItem foodItem in InventoryManager.Instance.GetFoodItems())
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemsContainer);
            ItemButton itemButton = buttonObj.GetComponent<ItemButton>();

            itemButton.Setup(foodItem, OnFoodSelected);
            itemButtons.Add(buttonObj);
        }

        // ���������� ������
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
            // ������ �������
            targetRabbit.Feed(foodItem);
        }

        HideInventory();
    }

    // ������� ��������� �� ������ ������
    public void OnCancelClicked()
    {
        HideInventory();
    }

    void Update()
    {
        // �������� ��������� ��� ����� ��� ���
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
