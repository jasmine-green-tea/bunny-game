using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ActionMenuManager : MonoBehaviour
{
    public static ActionMenuManager Instance;

    [Header("UI ��������")]
    public GameObject actionMenuPanel;
    public Button feedButton;
    // ������ ��������
    //public Button cancelButton;

    [Header("����������������")]
    public Vector2 menuOffset = new Vector2(0, 100f);

    private Rabbit currentRabbit;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        // ����������� ������
        feedButton.onClick.AddListener(OnFeedClicked);
        // ������ ��������
        //cancelButton.onClick.AddListener(HideActionMenu);

        // �������� ���� ��� ������
        HideActionMenu();
    }

    // �������� ���� �������� ��� �������
    public void ShowActionMenu(Rabbit rabbit)
    {
        currentRabbit = rabbit;

        // ������������� ���� ��� ��������
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(rabbit.transform.position);
        actionMenuPanel.transform.position = screenPosition + menuOffset;

        // ���������� ������ ��������� ��������
        feedButton.gameObject.SetActive(rabbit.canBeFed);
        // ������ ��������

        // ���������� ����
        actionMenuPanel.SetActive(true);
    }

    public void HideActionMenu()
    {
        actionMenuPanel.SetActive(false);
        currentRabbit = null;
    }

    private void OnFeedClicked()
    {

        // ���������� ��������� ��� ���������
        InventoryUI.Instance.ShowFeedingInventory(currentRabbit);
        HideActionMenu();
    }

    void Update()
    {
        // �������� ���� ��� ����� ��� ���
        if (actionMenuPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // ���������, ��� �� ���� ��� ����
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
}
