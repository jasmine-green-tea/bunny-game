using UnityEngine;
using UnityEngine.EventSystems;

public class Rabbit : MonoBehaviour, IPointerClickHandler
{
    [Header("Настройки кролика")]
    //public string rabbitName = "Кролик";
    [SerializeField] private string rabbitName;
    public NeedSystem needSystem;

    [Header("Визуальные элементы")]
    public GameObject selectionCircle;
    public Animator animator;

    [Header("Действия")]
    public bool canBeFed = true;
    // Другие действия

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

    public void Consume(InventoryItem inventoryItem)
    {
        if (needSystem != null && inventoryItem != null)
        {
            // здесь возможно будут анимации на определенное поведение
            switch (inventoryItem.itemType)
            {
                case ItemType.Food:
                    needSystem.IncreaseHunger(inventoryItem.restoreValue);
                    Debug.Log($"{rabbitName} покормлен на {inventoryItem.itemName}!");

                    break;
                case ItemType.Toy:
                    needSystem.IncreaseMood(inventoryItem.restoreValue);
                    Debug.Log($"{rabbitName} поигран на {inventoryItem.itemName}!");

                    break;
            }


            // Анимация

            // Убираем еду из инвентаря
            InventoryManager.Instance.RemoveItem(inventoryItem);
        }
    }

    void Update()
    {
        
    }
}
