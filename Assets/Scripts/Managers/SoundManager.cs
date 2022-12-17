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
        instance.musicSource.Play();
    }

    public void PlayBattleMusic()
    {
        instance.battleSource.Play();
        instance.musicSource.Stop();
    }

    public void ResumeMusic()
    {
        instance.battleSource.Stop();
        instance.musicSource.Play();
    }
}
