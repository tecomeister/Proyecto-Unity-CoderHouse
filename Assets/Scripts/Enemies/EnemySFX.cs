using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{

    [SerializeField] private AudioClip[] walkFootsteps;
    [SerializeField] private AudioClip[] runFootsteps;
    [SerializeField] private AudioClip[] hits;
    [SerializeField] private AudioClip[] deaths;
    [SerializeField] private AudioClip[] attacks;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void WalkFootstep()
    {
        AudioClip walkFootsteps = GetRandomWalkFootstep();
        audioSource.PlayOneShot(walkFootsteps);
    }

    private AudioClip GetRandomWalkFootstep()
    {
        return walkFootsteps[Random.Range(0, walkFootsteps.Length)];
    }

    private void RunFootsteps()
    {
        AudioClip runFootsteps = GetRandomRunFootstep();
        audioSource.PlayOneShot(runFootsteps);
    }

    private AudioClip GetRandomRunFootstep()
    {
        return runFootsteps[Random.Range(0, runFootsteps.Length)];
    }

    private void Attack()
    {
        AudioClip attacks = GetRandomAttack();
        audioSource.PlayOneShot(attacks);
    }

    private AudioClip GetRandomAttack()
    {
        return attacks[Random.Range(0, attacks.Length)];
    }


    public void Death()
    {
        AudioClip deaths = GetRandomDeath();
        audioSource.PlayOneShot(deaths);
    }

    private AudioClip GetRandomDeath()
    {
        return deaths[Random.Range(0, deaths.Length)];
    }


    private void Hit()
    {
        AudioClip hits = GetRandomHit();
        audioSource.PlayOneShot(hits);
    }

    private AudioClip GetRandomHit()
    {
        return hits[Random.Range(0, hits.Length)];
    }
}
