using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RabbitStats
{
    public string bunnyName;
    public float hungerRate;
    public float moodRate;
    public float hygieneRate;
    public InventoryItem likedItem;
    public InventoryItem dislikedItem;
    public BunnyColor bunnyColor;
    public int daysLeft;
    public int currentDays;
    public float sadTime;


    //public List<InventoryItem> likedItems = new List<InventoryItem>();
    //public List<InventoryItem> dislikedItems = new List<InventoryItem>();

}

public class Rabbit : PausableObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки кролика")]
    //public string rabbitName = "Кролик";
    [SerializeField] private string rabbitName;
    public NeedSystem needSystem;
    public RabbitStats rabbitStats;

    [Header("Визуальные элементы")]
    public GameObject selectionCircle;

    public bool interactable = true;
    // Другие действия

    private bool isInteracted = false;
    public RabbitController controller;

    
    public void SaveSadTime()
    {
        rabbitStats.sadTime = needSystem.sadTime;
        Debug.Log("sad time saved = " + rabbitStats.sadTime);
    }

    private void DisableInteraction()
    {
        interactable = false;
    }

    private void EnableInteraction()
    {
        interactable = true;
    }

    public void SetControllerColor()
    {
        controller.SetBunnyColor((int)rabbitStats.bunnyColor);
    }

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
        transform.GetChild(transform.childCount-1).GetComponent<TMP_Text>().text = newName;
    }

    public void SetNeedSystemMultipliers(float hungerRate, float moodRate, float hygieneRate)
    {
        needSystem.hungerDecreaseRate = hungerRate;
        needSystem.moodDecreaseRate = moodRate;
        needSystem.hygieneDecreaseRate = hygieneRate;
        needSystem.hygieneIncreaseRate = (int)Mathf.Ceil(hygieneRate);

    }

    void Start()
    {
        if (needSystem == null)
            needSystem = GetComponent<NeedSystem>();

        if (selectionCircle != null)
            selectionCircle.SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name != "Shadow")
                transform.GetChild(i).gameObject.SetActive(false);
        }


        controller.forcedAnimationEvent.AddListener(DisableInteraction);
        controller.releaseAnimationEvent.AddListener(EnableInteraction);

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);

        //RabbitManager.Instance.RegisterRabbit(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name != "Shadow")
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
            if (transform.GetChild(i).gameObject.name != "Shadow")
                transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!interactable)
            return;

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
            {
                multiplier = 1.5f;
                needSystem.IncreaseMood(10);

            }
            if (inventoryItem == rabbitStats.dislikedItem)
            {
                multiplier = 0.75f;
                needSystem.DecreaseMood(10);

            }

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
            if (transform.GetChild(i).gameObject.name != "Shadow")
                transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return null;
    }
}
