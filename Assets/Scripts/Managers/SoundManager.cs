using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioSource musicSource;
    public AudioSource battleSource;
    public AudioClip musicClip;
    public AudioClip battleClip;

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
            DontDestroyOnLoad(gameObject);
            instance.musicSource.clip = musicClip;
            instance.battleSource.clip = battleClip;
        }
        else
        {
            ResumeMusic();
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResumeMusic();
        instance.musicSource.Play();
        instance.battleSource.Play();
    }

    public void PlayBattleMusic()
    {
        instance.battleSource.volume = 0.5f;
        instance.musicSource.volume = 0f;
    }

    public void ResumeMusic()
    {
        instance.battleSource.volume = 0f;
        instance.musicSource.volume = 0.5f;
    }
}
