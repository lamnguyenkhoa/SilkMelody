using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InRangeInteractable : MonoBehaviour
{
    [SerializeField] protected bool playerInRange;
    [SerializeField] protected Player player;
    public GameObject interactText;
    protected InputMaster inputMaster;

    protected virtual void Awake()
    {
        inputMaster = new InputMaster();
    }

    protected virtual void OnEnable()
    {
        inputMaster.Enable();
    }

    protected virtual void OnDisable()
    {
        if (inputMaster != null)
            inputMaster.Disable();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            playerInRange = true;
            interactText.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}
