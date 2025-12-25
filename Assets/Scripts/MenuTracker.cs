using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTracker : PausableObject
{
    public GameObject ActionMenuPanel;
    public GameObject InventoryPanel;

    public static MenuTracker Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool GetMenuStatus()
    {
        return ActionMenuPanel.activeSelf || InventoryPanel.activeSelf;
    }

}
