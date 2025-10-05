using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemBatch
{
    public InventoryItem item;
    public int count;
}

[CreateAssetMenu(fileName = "NewInventoryInitializer", menuName = "Inventory/Inventory Initializer")]
public class InventoryInitializer : ScriptableObject
{
    public List<InventoryItemBatch> items;
}