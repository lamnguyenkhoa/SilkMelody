using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerStatSO playerStat;

    public void NewGameButton()
    {
        playerStat.ResetToNewGameState();
        SaveSystem.DeleteExistingSave();
        SceneManager.LoadScene("DirtCave0");
    }

    public void ContinueButton()
    {
        if (SaveSystem.LoadPlayerData(playerStat))
            SceneManager.LoadScene(playerStat.respawnScene);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}