using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject selectFrame;
    public AudioSource selectSound;

    private void Awake()
    {
        selectFrame.SetActive(false);
    }

    private void Start()
    {
        if (transform.name == "Continue")
        {
            TextMeshProUGUI continueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (!SaveSystem.CheckSaveExist())
            {
                Color noSaveColor = continueText.color;
                noSaveColor.a = 0.2f;
                continueText.color = noSaveColor;
            }
            else
            {
                EventSystem.current.firstSelectedGameObject = this.gameObject;
            }
        }
    }

    public void NewGameButton()
    {
        //SaveSystem.DeleteExistingSave();
        SceneManager.LoadScene("DirtCave0");
    }

    public void ContinueButton()
    {
        if (SaveSystem.CheckSaveExist())
        {
            SaveSystem.LoadPlayerData();
            SceneManager.LoadScene(GameMaster.instance.playerData.respawnScene);
        }
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
        selectSound.Play();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }
}
