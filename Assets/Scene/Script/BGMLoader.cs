using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMLoader : MonoBehaviour
{
    public AudioSource bgm;

    public static BGMLoader instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // If this instance has different bgm, change it!
            if (instance.bgm.clip != this.bgm.clip)
            {
                instance.bgm.clip = this.bgm.clip;
                instance.bgm.Play();
            }
            Destroy(this.gameObject);
        }
    }
}
