using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (transform.name == "Continue")
        {
            Button continueButton = GetComponent<Button>();
            if (!SaveSystem.CheckSaveExist())
            {
                continueButton.interactable = false;
            }
        }
    }

    public void NewGameButton()
    {
        SaveSystem.DeleteExistingSave();
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
}