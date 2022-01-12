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

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadingScreen(sceneName));
    }

    private IEnumerator LoadingScreen(string sceneName)
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}