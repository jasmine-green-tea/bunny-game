using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RabbitStats
{
    public float hungerRate;
    public float moodRate;
    public float hygieneRate;
    public List<InventoryItem> favouriteItems;
    public List<InventoryItem> hatedItems;

}

public class Rabbit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки кролика")]
    //public string rabbitName = "Кролик";
    [SerializeField] private string rabbitName;
    public NeedSystem needSystem;
    private RabbitStats rabbitStats;

    [Header("Визуальные элементы")]
    public GameObject selectionCircle;
    public Animator animator;

    [Header("Действия")]
    public bool canBeFed = true;
    // Другие действия

    private bool isInteracted = false;

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

    public void SetNeedSystemMultipliers(float hungerRate, float moodRate, float hygieneRate)
    {
        needSystem.hungerDecreaseRate = hungerRate;
        needSystem.moodDecreaseRate = moodRate;
        needSystem.hygieneDecreaseRate = hygieneRate;


    }

    void Start()
    {
        if (needSystem == null)
            needSystem = GetComponent<NeedSystem>();

        if (selectionCircle != null)
            selectionCircle.SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }


        //RabbitManager.Instance.RegisterRabbit(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        needSystem.UpdateUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (isInteracted)
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        // Показываем меню действий
        isInteracted = true;

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
                    //Debug.Log($"{rabbitName} покормлен на {inventoryItem.itemName}!");

                    break;
                case ItemType.Toy:
                    needSystem.IncreaseMood(inventoryItem.restoreValue);
                    //Debug.Log($"{rabbitName} поигран на {inventoryItem.itemName}!");

                    break;
            }


            isInteracted = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
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
