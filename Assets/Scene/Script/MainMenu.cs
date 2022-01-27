using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerStatSO playerStat;
    public WorldStateSO worldState;

    public void NewGameButton()
    {
        playerStat.ResetToNewGameState();
        worldState.ResetToNewGameState();
        SaveSystem.DeleteExistingSave();
        SceneManager.LoadScene("DirtCave0");
    }

    public void ContinueButton()
    {
        if (SaveSystem.LoadPlayerData(playerStat, worldState))
            SceneManager.LoadScene(playerStat.respawnScene);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}