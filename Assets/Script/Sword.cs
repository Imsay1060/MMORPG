using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sword : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject particleEffect; // Particle effect for sword slash (optional)
    [SerializeField] protected AudioSource swingAudio; // Audio source for sword sounds
    [SerializeField] protected AudioClip swingSound; // Default sound for sword swing

    // Sword stats
    [SerializeField] protected float damage = 10f; // Base damage
    [SerializeField] protected float range = 2f; // Attack range
    [SerializeField] protected float attackSpeed = 1f; // Attacks per second

    protected float cooldown;
    private float timer = 0;

    protected virtual void Start()
    {
        cooldown = 1f / attackSpeed; // Calculate cooldown based on attack speed
        timer = cooldown;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            timer += Time.deltaTime;
            if (Input.GetMouseButton(0) && timer > cooldown)
            {
                Attack();
            }
        }
    }

    public void Attack()
    {
        timer = 0;
        PlaySwingSound();
        OnAttack();
    }

    protected abstract void OnAttack();

    protected void PlaySwingSound()
    {
        if (swingAudio && swingSound)
        {
            swingAudio.PlayOneShot(swingSound);
            swingAudio.pitch = Random.Range(1f, 1.2f); // Slight variation in pitch
        }
    }
}

