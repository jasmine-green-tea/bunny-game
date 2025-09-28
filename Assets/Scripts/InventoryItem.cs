using UnityEngine;

public enum ItemType
{
    Food,
    Toy,
};

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Inventory Item")]
public class InventoryItem: ScriptableObject
{
    [Header("������� ���������")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public int restoreValue;
    public ItemType itemType;

    [Header("��������")]
    [TextArea] public string description;
}