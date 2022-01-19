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

    [Header("Other")]
    private Player player;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            player = GameObject.Find("Tenroh").GetComponent<Player>();
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
            backupRespawnPos = player.transform.position;
        }
    }

    private void UpdatePlayerPosition(Scene scene, LoadSceneMode mode)
    {
        // Update position because of death
        if (doRespawn)
        {
            if (respawnChairName == "")
            {
                player.transform.position = backupRespawnPos;
                doRespawn = false;
                player.rb.gravityScale = player.originalGravityScale;
            }
            else
            {
                RestChair chair = GameObject.Find(respawnChairName).GetComponent<RestChair>();
                player.transform.position = chair.transform.position;
                chair.RespawnAssignToChair(player.GetComponent<Player>());
                chair.GetOnChair();
                doRespawn = false;
            }
        }
        // Update position because of change scene
        else if (spawnPosName != "")
        {
            Transform target = GameObject.Find(spawnPosName).transform;
            player.transform.position = target.position;
            spawnPosName = "";
            player.rb.gravityScale = player.originalGravityScale;
        }
        player.disableControlCounter -= 1;
        if (player.disableControlCounter < 0)
            player.disableControlCounter = 0;
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
        player.disableControlCounter += 1;
        player.rb.gravityScale = 0f;
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