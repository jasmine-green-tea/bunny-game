using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class RabbitStats
{
    public string bunnyName;
    public float hungerRate;
    public float moodRate;
    public float hygieneRate;
    public InventoryItem likedItem;
    public InventoryItem dislikedItem;

    //public List<InventoryItem> likedItems = new List<InventoryItem>();
    //public List<InventoryItem> dislikedItems = new List<InventoryItem>();

}

public class Rabbit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки кролика")]
    //public string rabbitName = "Кролик";
    [SerializeField] private string rabbitName;
    public NeedSystem needSystem;
    public RabbitStats rabbitStats;

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

        StartCoroutine(WaitForMenus());

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

            float multiplier = 1f;

            if (inventoryItem == rabbitStats.likedItem)
                multiplier = 1.5f;
            if (inventoryItem == rabbitStats.dislikedItem)
                multiplier = 0.75f;

            switch (inventoryItem.itemType)
            {
                case ItemType.Food:
                    needSystem.IncreaseHunger((int)(inventoryItem.restoreValue*multiplier));
                    //Debug.Log($"{rabbitName} покормлен на {inventoryItem.itemName}!");

                    break;
                case ItemType.Toy:
                    needSystem.IncreaseMood((int)(inventoryItem.restoreValue * multiplier));
                    //Debug.Log($"{rabbitName} поигран на {inventoryItem.itemName}!");

                    break;
            }


            isInteracted = false;


            // Анимация

            // Убираем еду из инвентаря
            InventoryManager.Instance.RemoveItem(inventoryItem);
        }
    }

    void Update()
    {

    }

    IEnumerator WaitForMenus()
    {
        yield return new WaitWhile(() => isInteracted || MenuTracker.Instance.GetMenuStatus());

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return null;
    }
}
