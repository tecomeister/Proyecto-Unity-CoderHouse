using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] walkFootsteps;
    [SerializeField] private AudioClip[] runFootsteps;
    [SerializeField] private AudioClip[] jumps;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip attack;

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

    private void Jumps()
    {
        AudioClip jumps = GetRandomJump();
        audioSource.PlayOneShot(jumps);
    }

    private AudioClip GetRandomJump()
    {
        return jumps[Random.Range(0, jumps.Length)];
    }

    private void Attack()
    {
        audioSource.PlayOneShot(attack);
    }

    private void Hit()
    {
        audioSource.PlayOneShot(hit);
    }

    public void Death()
    {
        audioSource.PlayOneShot(death);
    }

}
