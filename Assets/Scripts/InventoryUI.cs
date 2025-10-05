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

    private Rabbit targetRabbit;
    private List<GameObject> itemButtons = new List<GameObject>();

    private GridLayoutGroup itemsContainerLayout;

    private void Awake()
    {
        Instance = this;
        if (itemsContainerLayout == null)
            itemsContainerLayout = itemsContainer.GetComponentInChildren<GridLayoutGroup>();
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    // �������� ��������� ��� ���������
    public void ShowInventory(Rabbit rabbit, Vector2 inventoryPosition, ItemType itemType)
    {
        targetRabbit = rabbit;

        // ������� ���������� ������
        ClearItemButtons();

        int itemCount = 0;


        foreach (KeyValuePair<InventoryItem, int> entry in InventoryManager.Instance.GetInventoryItemsMap())
        {
            if (entry.Key.itemType != itemType)
                continue;

            GameObject buttonObj = Instantiate(itemButtonPrefab, itemsContainer);
            ItemButton itemButton = buttonObj.GetComponent<ItemButton>();

            itemButton.Setup(entry.Key, entry.Value, OnItemSelected);
            itemButtons.Add(buttonObj);
            itemCount++;
        }

        inventoryPanel.transform.position = inventoryPosition;
        // ���������� ������
        inventoryPanel.SetActive(true);

        if (itemCount >= 3)
        {
            itemsContainerLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            itemsContainerLayout.constraintCount = 3;
        }
        else
        {
            itemsContainerLayout.constraint = GridLayoutGroup.Constraint.Flexible;
        }
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

    private void OnItemSelected(InventoryItem inventoryItem)
    {
        if (targetRabbit != null && inventoryItem != null)
        {
            // ������ �������
            targetRabbit.Consume(inventoryItem);
        }

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
