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
        SceneManager.sceneLoaded += SceneChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChange;
    }

    public void SceneChange(Scene scene, LoadSceneMode mode)
    {
        GameMaster.instance.PatchInventoryReference();
        GameMaster.instance.AddVisitedRoom(scene.name);
        UpdatePlayerPosition();
    }

    private void SpawnPointInit()
    {
        if (GameMaster.instance.playerData.respawnScene == "")
        {
            Debug.Log("Should never reach here");
            //GameMaster.instance.playerData.respawnScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            doRespawn = true;

            // For easier unit testing
            if (Application.isEditor)
            {
                Debug.Log("Reset player respawnPos to (0 0 0) and delete respawnChairName");
                GameMaster.instance.playerData.respawnPos = Vector3.zero;
                GameMaster.instance.playerData.respawnChairName = "";
            }

            // Edge case: When player New game and die before reach the first chair
            if (GameMaster.instance.playerData.respawnChairName == "" && GameMaster.instance.playerData.respawnPos == Vector3.zero)
            {
                GameMaster.instance.playerData.respawnPos = player.transform.position;
            }
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        // Update position because of death
        if (doRespawn)
        {
            // Not sit on any chair yet
            if (GameMaster.instance.playerData.respawnChairName == "")
            {
                player.transform.position = GameMaster.instance.playerData.respawnPos;
                player.rb.isKinematic = false;
            }
            else
            {
                RestChair chair = GameObject.Find(GameMaster.instance.playerData.respawnChairName).GetComponent<RestChair>();
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
        GameMaster.instance.playerData.respawnChairName = restChairName;
        GameMaster.instance.playerData.respawnScene = SceneManager.GetActiveScene().name;
    }

    public void Respawn()
    {
        doRespawn = true;
        LoadLevel(GameMaster.instance.playerData.respawnScene, "");
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
