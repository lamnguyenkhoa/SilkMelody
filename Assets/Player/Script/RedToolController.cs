using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedToolController : MonoBehaviour
{
    private Player player;
    public Projectile throwBladeProj;
    public Projectile tripleKnifeProj;
    public GameObject lifebloodNeedlePE;

    private void Start()
    {
        player = GameObject.Find("Tenroh").GetComponent<Player>();
    }

    public void UseRedTool(RedTool.ToolName toolName)
    {
        switch (toolName)
        {
            case RedTool.ToolName.throwBlade:
                UseThrowBlade();
                break;

            case RedTool.ToolName.trippleKnife:
                UseTripleKnife();
                break;

            case RedTool.ToolName.lifebloodNeedle:
                UseLifeBloodNeedle();
                break;

            default:
                Debug.Log("Not coded yet!");
                break;
        }
    }

    public void UseThrowBlade()
    {
        if (GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.throwBlade] > 0)
        {
            Projectile newProj = Instantiate(throwBladeProj, player.transform.position, Quaternion.identity);
            if (player.isFacingLeft)
            {
                newProj.transform.localScale = new Vector3(-newProj.transform.localScale.x, newProj.transform.localScale.y, newProj.transform.localScale.z);
                newProj.GetComponent<Rigidbody2D>().velocity = new Vector2(-20f, 0);
            }
            else
                newProj.GetComponent<Rigidbody2D>().velocity = new Vector2(20f, 0);
            GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.throwBlade] -= 1;
            GetComponent<PlayerSoundEffect>().PlaySoundEffect(PlayerSoundEffect.SoundEnum.throwing);
        }
    }

    public void UseTripleKnife()
    {
        if (GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.trippleKnife] > 0)
        {
            Vector2 straightDirection = Vector2.right;
            Vector2 spreadDirection1 = Quaternion.Euler(0, 0, 10f) * straightDirection;
            Vector2 spreadDirection2 = Quaternion.Euler(0, 0, -10) * straightDirection;

            Projectile newProj = Instantiate(tripleKnifeProj, player.transform.position, Quaternion.identity);
            Projectile newProj2 = Instantiate(tripleKnifeProj, player.transform.position, Quaternion.identity);
            Projectile newProj3 = Instantiate(tripleKnifeProj, player.transform.position, Quaternion.identity);

            if (player.isFacingLeft)
            {
                newProj.transform.localScale = new Vector3(-newProj.transform.localScale.x, newProj.transform.localScale.y, newProj.transform.localScale.z);
                newProj2.transform.localScale = newProj.transform.localScale;
                newProj3.transform.localScale = newProj.transform.localScale;

                newProj.GetComponent<Rigidbody2D>().velocity = -straightDirection * 20f;
                newProj2.GetComponent<Rigidbody2D>().velocity = -spreadDirection1 * 20f;
                newProj3.GetComponent<Rigidbody2D>().velocity = -spreadDirection2 * 20f;
            }
            else
            {
                newProj.GetComponent<Rigidbody2D>().velocity = straightDirection * 20f;
                newProj2.GetComponent<Rigidbody2D>().velocity = spreadDirection1 * 20f;
                newProj3.GetComponent<Rigidbody2D>().velocity = spreadDirection2 * 20f;
            }
            GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.trippleKnife] -= 1;
            GetComponent<PlayerSoundEffect>().PlaySoundEffect(PlayerSoundEffect.SoundEnum.throwing);
        }
    }

    public void UseLifeBloodNeedle()
    {
        if (GameMaster.instance.playerData.lifebloodHp > 0)
            return;

        if (GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.lifebloodNeedle] > 0)
        {
            GameMaster.instance.playerData.redToolsCurrentCharge[(int)RedTool.ToolName.lifebloodNeedle] -= 1;
            player.anim.SetTrigger("useLifebloodNeedle");
            GetComponent<PlayerSoundEffect>().PlaySoundEffect(PlayerSoundEffect.SoundEnum.throwing);
        }

    }
}
