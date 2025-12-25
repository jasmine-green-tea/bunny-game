using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ActionMenuManager : PausableObject
{
    public static ActionMenuManager Instance;

    [Header("UI элементы")]
    public GameObject actionMenuPanel;
    public Button feedButton;
    public Button playButton;
    // Другие действия
    //public Button cancelButton;

    [Header("Позиционирование")]
    public Vector2 menuOffset = new Vector2(0, 100f);
    private Vector2 inventoryPosition = Vector2.zero; 

    private Rabbit currentRabbit;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // Настраиваем кнопки
        feedButton.onClick.AddListener(OnFeedClicked);
        playButton.onClick.AddListener(OnPlayClicked);
        // Другие действия
        //cancelButton.onClick.AddListener(HideActionMenu);

        // Скрываем меню при старте
        HideActionMenu();

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);
    }

    // Показать меню действий для кролика
    public void ShowActionMenu(Rabbit rabbit)
    {
        currentRabbit = rabbit;

        // Позиционируем меню над кроликом
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(rabbit.transform.position);
        actionMenuPanel.transform.position = screenPosition + menuOffset;
        inventoryPosition = screenPosition + menuOffset;

        // Активируем только доступные действия
        feedButton.gameObject.SetActive(true);
        // Другие действия

        // Показываем меню
        actionMenuPanel.SetActive(true);
    }

    public void HideActionMenu()
    {
        actionMenuPanel.SetActive(false);
        currentRabbit = null;
    }

    private void OnFeedClicked()
    {
        // Показываем инвентарь для кормления
        InventoryUI.Instance.ShowInventory(currentRabbit, inventoryPosition, ItemType.Food);
        HideActionMenu();
    }

    private void OnPlayClicked()
    {
        // Показываем инвентарь для игр
        InventoryUI.Instance.ShowInventory(currentRabbit, inventoryPosition, ItemType.Toy);
        HideActionMenu();
    }

    protected override void UpdatePausable(float deltaTime)
    {
        // Скрывать меню при клике вне его
        if (actionMenuPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Проверяем, был ли клик вне меню
            if (!IsPointerOverUIElement())
            {
                HideActionMenu();
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    private void OnDestroy()
    {
        if (PauseManager.Instance != null)
            PauseManager.Instance.UnregisterPausable(this);
    }
}
