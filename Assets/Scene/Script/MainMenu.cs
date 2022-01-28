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
        StartCoroutine(NewGame());
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

    private IEnumerator NewGame()
    {
        playerStat.ResetToNewGameState();
        worldState.ResetToNewGameState();
        SaveSystem.DeleteExistingSave();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("DirtCave0");
    }
}