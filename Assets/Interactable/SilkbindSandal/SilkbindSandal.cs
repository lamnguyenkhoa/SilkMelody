using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SilkbindSandal : MonoBehaviour
{
    private bool used;
    private WorldData worldState;
    public float tutorialMinTime = 3f;
    public GameObject tutorial;
    public GameObject promt;
    private InputMaster inputMaster;
    private AudioSource sound;
    private CanvasGroup canvasGroup;
    public float fadeSpeed = 0.3f;

    private void Awake()
    {
        worldState = GameMaster.instance.worldData;
        if (worldState.doubleJump)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        inputMaster = new InputMaster();
        inputMaster.Enable();
        sound = GetComponent<AudioSource>();
        canvasGroup = tutorial.GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !used)
        {
            used = true;
            worldState.doubleJump = true;
            GameMaster.instance.playerData.inventoryItemAmount[(int)InventoryItem.ItemName.silkbindSandal] += 1;
            player.playerStat.extraJump += 1;
            StartCoroutine(DoubleJumpTutorial(player));
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator DoubleJumpTutorial(Player player)
    {
        canvasGroup.alpha = 0;
        player.disableControlCounter += 1;
        promt.SetActive(false);
        tutorial.SetActive(true);
        sound.Play();

        // Fade in tutorial
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(tutorialMinTime);
        promt.SetActive(true);
        InputAction jumpAction = inputMaster.Gameplay.Jump;
        while (!jumpAction.IsPressed())
        {
            yield return null;
        }

        // Fade out
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        tutorial.SetActive(false);
        player.disableControlCounter -= 1;
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
