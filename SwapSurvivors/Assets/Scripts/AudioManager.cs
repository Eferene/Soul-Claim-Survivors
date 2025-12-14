using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource sfxSource;
    [SerializeField] List<AudioClip> enemyHurtSFXClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> playerLeftStepSFXClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> playerRightStepSFXClips = new List<AudioClip>();
    bool isLeft;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            sfxSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEnemyHurtSFX()
    {
        AudioClip clip = enemyHurtSFXClips[Random.Range(0, enemyHurtSFXClips.Count)];
        sfxSource.pitch = Random.Range(0.5f, 1.5f);
        sfxSource.PlayOneShot(clip, 0.5f);
    }

    public void PlayPlayerStepSFX()
    {
        AudioClip clip;
        if(isLeft) clip = playerLeftStepSFXClips[Random.Range(0, playerLeftStepSFXClips.Count)];
        else clip = playerRightStepSFXClips[Random.Range(0, playerRightStepSFXClips.Count)];
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(clip, 0.35f);
    }
}
