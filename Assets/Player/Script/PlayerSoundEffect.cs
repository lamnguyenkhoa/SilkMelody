using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    [Header("Footstep")]
    public float footstepFrequency;
    [SerializeField] private AudioSource footstep;
    private float footstepTimer;

    [Header("Other")]
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource damaged;
    [SerializeField] private AudioSource parry;
    [SerializeField] private AudioSource dash;
    [SerializeField] private AudioSource silkbind;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource landing;
    [SerializeField] private AudioSource throwing;
    [SerializeField] private AudioSource syringe;

    private Player player;

    public enum SoundEnum
    { attack, damaged, parry, silkbind, footstep, dash, jump, landing, throwing, syringe }

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
            PlaySoundEffect(SoundEnum.footstep);
        }
    }

    public void PlaySoundEffect(SoundEnum soundName)
    {
        AudioSource soundSource = attack;
        switch (soundName)
        {
            case SoundEnum.attack:
                soundSource = attack;
                break;

            case SoundEnum.damaged:
                soundSource = damaged;
                break;

            case SoundEnum.parry:
                soundSource = parry;
                break;

            case SoundEnum.silkbind:
                soundSource = silkbind;
                break;

            case SoundEnum.footstep:
                soundSource = footstep;
                break;

            case SoundEnum.dash:
                soundSource = dash;
                break;

            case SoundEnum.jump:
                soundSource = jump;
                break;

            case SoundEnum.landing:
                soundSource = landing;
                break;

            case SoundEnum.throwing:
                soundSource = throwing;
                break;

            case SoundEnum.syringe:
                soundSource = syringe;
                break;
        }
        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        soundSource.volume = randomVolume;
        soundSource.pitch = randomPitch;
        soundSource.Play();
    }
}
