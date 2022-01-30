using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryMenu : MonoBehaviour
{
    private Player player;
    public GameObject inventoryHolder;
    public GameObject[] inventoryTabs;
    private InputMaster inputMaster;

    private InputAction openMenuAction;
    private InputAction closeMenuAction;
    private InputAction leftTabAction;
    private InputAction rightTabAction;

    private int currentTabIndex;

    private void Start()
    {
        player = GameObject.Find("Tenroh").GetComponent<Player>();
        inventoryHolder.SetActive(false);

        openMenuAction = inputMaster.Gameplay.InventoryMenu;
        closeMenuAction = inputMaster.Inventory.OutOfMenu;
        leftTabAction = inputMaster.Inventory.LeftTab;
        rightTabAction = inputMaster.Inventory.RightTab;
    }

    private void OnEnable()
    {
        inputMaster = new InputMaster();
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Update()
    {
        // Open menu
        if (openMenuAction.WasPressedThisFrame() && !player.inMenu)
        {
            inventoryHolder.SetActive(true);
            player.inMenu = true;
            currentTabIndex = 0;
        }
        // Close menu
        else if (closeMenuAction.WasPressedThisFrame() && player.inMenu)
        {
            inventoryHolder.SetActive(false);
            player.inMenu = false;
        }

        if (player.isHurt)
        {
            inventoryHolder.SetActive(false);
            player.inMenu = false;
        }

        // Control tab
        if (player.inMenu && inventoryHolder.activeSelf)
        {
            if (leftTabAction.WasPressedThisFrame())
            {
                currentTabIndex--;
                if (currentTabIndex < 0)
                    currentTabIndex = 2;
                UpdateInventoryTab();
            }
            else if (rightTabAction.WasPressedThisFrame())
            {
                currentTabIndex++;
                if (currentTabIndex > 2)
                    currentTabIndex = 0;
                UpdateInventoryTab();
            }
        }
    }

    private void UpdateInventoryTab()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == currentTabIndex)
            {
                inventoryTabs[i].SetActive(true);
            }
            else
            {
                inventoryTabs[i].SetActive(false);
            }
        }
    }
}