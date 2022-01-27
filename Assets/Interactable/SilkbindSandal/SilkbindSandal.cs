using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SilkbindSandal : MonoBehaviour
{
    private bool used;
    public WorldStateSO worldState;
    public float tutorialMinTime = 3f;
    public GameObject tutorial;
    public GameObject promt;
    private InputMaster inputMaster;

    private void Awake()
    {
        if (worldState.doubleJump)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        inputMaster = new InputMaster();
        inputMaster.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !used)
        {
            used = true;
            worldState.doubleJump = true;
            player.playerStat.extraJump += 1;
            StartCoroutine(DoubleJumpTutorial(player));
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator DoubleJumpTutorial(Player player)
    {
        player.disableControlCounter += 1;
        promt.SetActive(false);
        tutorial.SetActive(true);
        yield return new WaitForSeconds(tutorialMinTime);
        promt.SetActive(true);
        InputAction jumpAction = inputMaster.Gameplay.Jump;
        while (!jumpAction.IsPressed())
        {
            yield return null;
        }
        tutorial.SetActive(false);
        player.disableControlCounter -= 1;
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}