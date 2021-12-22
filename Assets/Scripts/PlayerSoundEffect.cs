using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource damaged;

    public void PlayAttackSound()
    {
        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        attack.volume = randomVolume;
        attack.pitch = randomPitch;
        attack.Play();
    }

    public void PlayDamagedSound()
    {
        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        damaged.volume = randomVolume;
        damaged.pitch = randomPitch;
        damaged.Play();
    }
}