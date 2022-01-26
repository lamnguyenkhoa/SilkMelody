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
        if (player.playerStat.respawnScene == "")
        {
            Debug.Log("Should never reach here");
            //player.playerStat.respawnScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            doRespawn = true;

            // For easier unit testing
            if (Application.isEditor)
            {
                Debug.Log("Reset player respawnPos to (0 0 0)");
                player.playerStat.respawnPos = Vector3.zero;
            }

            // Edge case: When player New game and die before reach the first chair
            if (player.playerStat.respawnChairName == "" && player.playerStat.respawnPos == Vector3.zero)
            {
                player.playerStat.respawnPos = player.transform.position;
            }
            UpdatePlayerPosition(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    // Parameters are unused
    private void UpdatePlayerPosition(Scene scene, LoadSceneMode mode)
    {
        // Update position because of death
        if (doRespawn)
        {
            // Not sit on any chair yet
            if (player.playerStat.respawnChairName == "")
            {
                player.transform.position = player.playerStat.respawnPos;
                player.rb.isKinematic = false;
            }
            else
            {
                RestChair chair = GameObject.Find(player.playerStat.respawnChairName).GetComponent<RestChair>();
                player.transform.position = chair.transform.position;
                chair.RespawnAssignToChair(player.GetComponent<Player>());
                chair.GetOnChair();
            }
            doRespawn = false;
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
        player.playerStat.respawnChairName = restChairName;
        player.playerStat.respawnScene = SceneManager.GetActiveScene().name;
    }

    public void Respawn()
    {
        doRespawn = true;
        LoadLevel(player.playerStat.respawnScene, "");
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