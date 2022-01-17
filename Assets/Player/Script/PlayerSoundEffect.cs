using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    [Header("Footstep")]
    public float footstepFrequency;
    [SerializeField] private AudioSource footstepSound;
    private float footstepTimer;

    [Header("Other")]
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource damaged;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        HandleFootstepSound();
    }

    private void HandleFootstepSound()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepFrequency && player.isGrounded && player.state == Player.State.running)
        {
            footstepTimer = 0f;
            PlayFootstepSound();
        }
    }

    public void PlayFootstepSound()
    {
        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        footstepSound.volume = randomVolume;
        footstepSound.pitch = randomPitch;
        footstepSound.Play();
    }

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