using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator anim;
    public float transitionTime = 1f;
    public static LevelLoader instance;
    public string spawnPosName = "";

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdatePlayerPosition;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdatePlayerPosition;
    }

    private void UpdatePlayerPosition(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DitMeUnityGayVL());
    }

    public void LoadLevel(string sceneName, string spawnPosName)
    {
        this.spawnPosName = spawnPosName;
        StartCoroutine(LoadingScreen(sceneName));
    }

    private IEnumerator DitMeUnityGayVL()
    {
        // If we dont wait for 0.1f, player will be reset to scene original position
        yield return new WaitForSeconds(0.1f);
        if (spawnPosName != "")
        {
            Transform player = GameObject.Find("Tenroh").transform;
            Transform target = GameObject.Find(spawnPosName).transform;
            player.position = target.position;
            spawnPosName = "";
        }
    }

    private IEnumerator LoadingScreen(string sceneName)
    {
        anim.SetBool("changeScene", true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        anim.SetBool("changeScene", false);
    }
}