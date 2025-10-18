using UnityEngine;
using UnityEngine.EventSystems;

public class Rabbit : MonoBehaviour, IPointerClickHandler
{
    [Header("��������� �������")]
    //public string rabbitName = "������";
    [SerializeField] private string rabbitName;
    public NeedSystem needSystem;

    [Header("���������� ��������")]
    public GameObject selectionCircle;
    public Animator animator;

    [Header("��������")]
    public bool canBeFed = true;
    // ������ ��������

    public void SetPausedStatus(bool paused)
    {
        needSystem.Pause(paused);
    }

    public string GetName()
    {
        return rabbitName;
    }

    public void SetName(string newName)
    {
        rabbitName = newName;
    }

    void Start()
    {
        if (needSystem == null)
            needSystem = GetComponent<NeedSystem>();

        if (selectionCircle != null)
            selectionCircle.SetActive(false);

        RabbitManager.Instance.RegisterRabbit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"���� �� �������: {rabbitName}");

        // ���������� ���� ��������
        ActionMenuManager.Instance.ShowActionMenu(this);

        // ����������, �������� ���� ���������
        //ToggleSelection();
    }

    public void ToggleSelection()
    {
        if (selectionCircle != null)
        {
            bool isSelected = !selectionCircle.activeSelf;
            selectionCircle.SetActive(isSelected);
        }
    }

    public void Deselect()
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(false);
    }

    public void Consume(InventoryItem inventoryItem)
    {
        if (needSystem != null && inventoryItem != null)
        {
            // ����� �������� ����� �������� �� ������������ ���������
            switch (inventoryItem.itemType)
            {
                case ItemType.Food:
                    needSystem.IncreaseHunger(inventoryItem.restoreValue);
                    Debug.Log($"{rabbitName} ��������� �� {inventoryItem.itemName}!");

                    break;
                case ItemType.Toy:
                    needSystem.IncreaseMood(inventoryItem.restoreValue);
                    Debug.Log($"{rabbitName} ������� �� {inventoryItem.itemName}!");

                    break;
            }


            // ��������

            // ������� ��� �� ���������
            InventoryManager.Instance.RemoveItem(inventoryItem);
        }
    }

    void Update()
    {
        
    }
}
