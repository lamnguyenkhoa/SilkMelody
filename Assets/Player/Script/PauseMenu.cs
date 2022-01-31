using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private Player player;
    public GameObject pauseHolder;
    private InputMaster inputMaster;
    private InputAction pauseMenuAction;

    private void Start()
    {
        player = GameObject.Find("Tenroh").GetComponent<Player>();
        pauseHolder.SetActive(false);

        pauseMenuAction = inputMaster.Gameplay.Pause;
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
        // Pause
        if (pauseMenuAction.WasPressedThisFrame() && !pauseHolder.activeSelf)
        {
            pauseHolder.SetActive(true);
            player.disableControlCounter += 1;
            Time.timeScale = 0f;
        }
        // Unpause
        else if (pauseMenuAction.WasPressedThisFrame() && pauseHolder.activeSelf)
        {
            pauseHolder.SetActive(false);
            player.disableControlCounter -= 1;
            Time.timeScale = 1f;
        }
    }
}