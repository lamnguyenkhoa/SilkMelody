using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryMenu : MonoBehaviour
{
    private Player player;
    public GameObject inventoryHolder;
    public GameObject[] inventoryMenuTabs;
    private InputMaster inputMaster;

    private InputAction openMenuAction;
    private InputAction closeMenuAction;
    private InputAction leftTabAction;
    private InputAction rightTabAction;

    public AudioSource openInventorySound;
    public AudioSource closeInventorySound;
    public AudioSource movingButtonSound;
    public AudioSource pressedButtonSound;

    private int currentTabIndex;

    public static InventoryMenu instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
        if (inputMaster != null)
            inputMaster.Disable();
    }

    private void Update()
    {
        // Open menu
        if (openMenuAction.WasPressedThisFrame() && !player.inMenu)
        {
            openInventorySound.Play();
            inventoryHolder.SetActive(true);
            player.inMenu = true;
            currentTabIndex = 1;
            UpdateInventoryMenuTab();
        }
        // Close menu
        else if (closeMenuAction.WasPressedThisFrame() && player.inMenu)
        {
            closeInventorySound.Play();
            inventoryHolder.SetActive(false);
            player.inMenu = false;
        }

        if (player.isHurt)
            CloseMenu();

        // Control tab
        if (player.inMenu && inventoryHolder.activeSelf)
        {
            if (leftTabAction.WasPressedThisFrame())
            {
                currentTabIndex--;
                if (currentTabIndex < 0)
                    currentTabIndex = 2;
                UpdateInventoryMenuTab();
            }
            else if (rightTabAction.WasPressedThisFrame())
            {
                currentTabIndex++;
                if (currentTabIndex > 2)
                    currentTabIndex = 0;
                UpdateInventoryMenuTab();
            }
        }
    }

    public void CloseMenu()
    {
        inventoryHolder.SetActive(false);
        player.inMenu = false;
    }

    private void UpdateInventoryMenuTab()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == currentTabIndex)
            {
                inventoryMenuTabs[i].SetActive(true);
            }
            else
            {
                inventoryMenuTabs[i].SetActive(false);
            }
        }
    }
}
