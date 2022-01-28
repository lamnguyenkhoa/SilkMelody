using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayerStatSO playerStat;
    public WorldStateSO worldState;

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
        playerStat.ResetToNewGameState();
        worldState.ResetToNewGameState();
        UnityEditor.EditorUtility.SetDirty(playerStat);
        UnityEditor.EditorUtility.SetDirty(worldState);
        SaveSystem.DeleteExistingSave();
        SceneManager.LoadScene("DirtCave0");
    }

    public void ContinueButton()
    {
        if (SaveSystem.CheckSaveExist())
        {
            SaveSystem.LoadPlayerData(playerStat, worldState);
            SceneManager.LoadScene(playerStat.respawnScene);
        }
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}