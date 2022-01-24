using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerStatSO playerStat;

    public void NewGameButton()
    {
        SceneManager.LoadScene("DirtCave0");
    }

    public void ContinueButton()
    {
        // SaveSystem.LoadPlayerData(playerStat);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}