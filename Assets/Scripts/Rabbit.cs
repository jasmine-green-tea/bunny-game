using UnityEngine;
using UnityEngine.EventSystems;

public class Rabbit : MonoBehaviour, IPointerClickHandler
{
    [Header("��������� �������")]
    public string rabbitName = "������";
    public NeedSystem needSystem;

    [Header("���������� ��������")]
    public GameObject selectionCircle;
    public Animator animator;

    [Header("��������")]
    public bool canBeFed = true;
    // ������ ��������

    public string GetName()
    {
        return rabbitName;
    }

    void Start()
    {
        if (needSystem == null)
            needSystem = GetComponent<NeedSystem>();

        if (selectionCircle != null)
            selectionCircle.SetActive(false);
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

    public void Feed(FoodItem foodItem)
    {
        if (needSystem != null && foodItem != null)
        {
            needSystem.IncreaseHunger(foodItem.hungerRestore);
            Debug.Log($"{rabbitName} ��������� �� {foodItem.foodName}!");

            // ��������

            // ������� ��� �� ���������
            InventoryManager.Instance.RemoveItem(foodItem);
        }
    }

    void Update()
    {
        
    }
}
