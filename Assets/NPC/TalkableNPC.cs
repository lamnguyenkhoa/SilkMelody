using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkableNPC : InRangeInteractable
{
    [SerializeField] private DialogueGroup[] conversations;
    private int conversationId; // what are they talking about
    private int dialogueId; // which line or who talk in a conversation
    private bool isTalking;

    protected override void Start()
    {
        base.Start();
        // Handle conversationId depend on condition
        conversationId = 0;
    }

    private void Update()
    {
        HandleTalking();
    }

    private void HandleTalking()
    {
        bool pressUp = inputMaster.Gameplay.Movement.ReadValue<Vector2>().y == 1;
        if (playerInRange && pressUp && !player.inMenu && !isTalking)
        {
            isTalking = true;
            StopAllCoroutines();
            StartCoroutine(Talking());
            interactText.SetActive(false);
        }
    }

    private IEnumerator Talking()
    {
        player.DisableGameplayControl(true);
        player.disableControlCounter += 1;
        GameObject dialogueBox = GameObject.Find("DialogueCanvas").transform.GetChild(0).gameObject;
        dialogueBox.SetActive(true);
        TextMeshProUGUI dialogueText = dialogueBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        dialogueId = 0;
        string speaker = conversations[conversationId].dialogues[dialogueId].speaker;
        string content = conversations[conversationId].dialogues[dialogueId].content;
        dialogueText.text = "[" + speaker + "]\n" + content;

        while (true)
        {
            if (inputMaster.Dialogue.Continue.WasPressedThisFrame())
            {
                dialogueId += 1;
                if (dialogueId >= conversations[conversationId].dialogues.Length)
                {
                    break;
                }
                else
                {
                    speaker = conversations[conversationId].dialogues[dialogueId].speaker;
                    content = conversations[conversationId].dialogues[dialogueId].content;
                    dialogueText.text = "[" + speaker + "]\n" + content;
                }
            }
            yield return null;
        }
        player.disableControlCounter -= 1;
        player.DisableGameplayControl(false);
        dialogueBox.gameObject.SetActive(false);
        isTalking = false;
        interactText.SetActive(true);
    }
}
