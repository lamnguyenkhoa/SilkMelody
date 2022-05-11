using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossRoomEntry : MonoBehaviour
{
    public GameObject doorObject;
    public AudioSource bossBGM;
    public CinemachineConfiner2D cameraConfiner;
    public PolygonCollider2D originalConfineArea;
    public PolygonCollider2D bossRoomConfineArea;
    public GameObject boss;
    private bool defeatedBoss;
    private bool activated;
    private WorldData worldState;
    public WorldData.BossEnum bossEnum;

    private void Start()
    {
        worldState = GameMaster.instance.worldData;
        // Failsafe, make sure stuff are inactived
        doorObject.SetActive(false);
        bossBGM.Stop();
        boss.SetActive(false);

        if (worldState.bossDefeated[(int)bossEnum])
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (boss.GetComponent<Enemy>().isDead)
            DefeatBossFight();

        if (defeatedBoss)
            this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!defeatedBoss && !activated)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                doorObject.SetActive(true);
                GameMaster.instance.bgm.Pause();
                cameraConfiner.m_BoundingShape2D = bossRoomConfineArea;
                BGMLoader.instance.bgm.Pause();
                bossBGM.Play();
                boss.SetActive(true);
                activated = true;
            }
        }
    }

    public void DefeatBossFight()
    {
        doorObject.SetActive(false);
        GameMaster.instance.bgm.UnPause();
        BGMLoader.instance.bgm.Play();
        bossBGM.Stop();
        cameraConfiner.m_BoundingShape2D = originalConfineArea;
        defeatedBoss = true;
    }
}
