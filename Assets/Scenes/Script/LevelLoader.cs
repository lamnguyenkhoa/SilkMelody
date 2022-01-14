using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
    [Header("LevelTransition")]
    public Animator anim;
    public float transitionTime = 1f;
    public static LevelLoader instance;
    public string spawnPosName = "";

    [Header("Respawn")]
    public string respawnChairName = "";
    public string respawnScene = "";
    private Vector2 backupRespawnPos;
    private bool doRespawn;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SpawnPointInit();
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

    private void SpawnPointInit()
    {
        // Load function here
        // ...
        if (respawnScene == "")
        {
            respawnScene = SceneManager.GetActiveScene().name;
            Transform player = GameObject.Find("Tenroh").transform;
            backupRespawnPos = player.position;
        }
    }

    private void UpdatePlayerPosition(Scene scene, LoadSceneMode mode)
    {
        // Update position because of death
        if (doRespawn)
        {
            if (respawnChairName == "")
            {
                Transform player = GameObject.Find("Tenroh").transform;
                player.position = backupRespawnPos;
                doRespawn = false;
            }
            else
            {
                Transform player = GameObject.Find("Tenroh").transform;
                RestChair chair = GameObject.Find(respawnChairName).GetComponent<RestChair>();
                player.position = chair.transform.position;
                chair.RespawnAssignToChair(player.GetComponent<Player>());
                chair.SitOnChair();
                doRespawn = false;
            }
        }
        // Update position because of change scene
        else if (spawnPosName != "")
        {
            Transform player = GameObject.Find("Tenroh").transform;
            Transform target = GameObject.Find(spawnPosName).transform;
            player.position = target.position;
            spawnPosName = "";
        }
    }

    public void UpdateSpawnPoint(string restChairName)
    {
        respawnChairName = restChairName;
        respawnScene = SceneManager.GetActiveScene().name;
    }

    public void Respawn()
    {
        doRespawn = true;
        LoadLevel(respawnScene, "");
    }

    public void LoadLevel(string sceneName, string spawnPosName)
    {
        this.spawnPosName = spawnPosName;
        StartCoroutine(LoadingScreen(sceneName));
    }

    private IEnumerator LoadingScreen(string sceneName)
    {
        anim.SetBool("changeScene", true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        anim.SetBool("changeScene", false);
    }
}