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
    private InputAction openMapAction;

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
        openMapAction = inputMaster.Gameplay.Map;
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
        if (player.disableInventoryMenu)
        {
            return;
        }

        // Open menu - talisman
        if (openMenuAction.WasPressedThisFrame() && !player.inMenu)
        {
            GetComponent<Canvas>().enabled = true;
            openInventorySound.Play();
            inventoryHolder.SetActive(true);
            player.inMenu = true;
            currentTabIndex = 1;
            UpdateInventoryMenuTab();
        }
        // Open menu - map
        else if (openMapAction.WasPressedThisFrame() && !player.inMenu)
        {
            GetComponent<Canvas>().enabled = true;
            openInventorySound.Play();
            inventoryHolder.SetActive(true);
            player.inMenu = true;
            currentTabIndex = 2;
            UpdateInventoryMenuTab();
        }
        // Close menu
        else if (closeMenuAction.WasPressedThisFrame() && player.inMenu)
        {
            closeInventorySound.Play();
            inventoryHolder.SetActive(false);
            player.inMenu = false;

            GetComponent<Canvas>().enabled = false;
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
                    currentTabIndex = inventoryMenuTabs.Length - 1;
                UpdateInventoryMenuTab();
                openInventorySound.Play();
            }
            else if (rightTabAction.WasPressedThisFrame())
            {
                currentTabIndex++;
                if (currentTabIndex > (inventoryMenuTabs.Length - 1))
                    currentTabIndex = 0;
                UpdateInventoryMenuTab();
                openInventorySound.Play();
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
        for (int i = 0; i < inventoryMenuTabs.Length; i++)
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
