using UnityEngine;
using UnityEngine.EventSystems;

public class Rabbit : MonoBehaviour, IPointerClickHandler
{
    [Header("Настройки кролика")]
    public string rabbitName = "Кролик";
    public NeedSystem needSystem;

    [Header("Визуальные элементы")]
    public GameObject selectionCircle;
    public Animator animator;

    [Header("Действия")]
    public bool canBeFed = true;
    // Другие действия

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
        Debug.Log($"Клик по кролику: {rabbitName}");

        // Показываем меню действий
        ActionMenuManager.Instance.ShowActionMenu(this);

        // Показываем, скрываем круг выделения
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
            Debug.Log($"{rabbitName} покормлен на {foodItem.foodName}!");

            // Анимация

            // Убираем еду из инвентаря
            InventoryManager.Instance.RemoveItem(foodItem);
        }
    }

    void Update()
    {
        
    }
}
