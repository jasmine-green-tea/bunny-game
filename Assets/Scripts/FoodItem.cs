using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodItem", menuName = "Inventory/Food Item")]
public class FoodItem : ScriptableObject
{
    [Header("��������� ���")]
    public string foodName = "��������";
    public int hungerRestore = 30;
    public Sprite itemIcon;
    public GameObject foodPrefab; // ������ ��� ������������

    [Header("��������")]
    [TextArea] public string description;
}
