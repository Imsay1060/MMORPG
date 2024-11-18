using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : Sword
{
    [SerializeField] private float dashDistance = 1.0f; // Distance to dash forward on each slash

    protected override void Start()
    {
        base.Start();
        damage = 12f; // Moderate damage
        range = 2f; // Medium range
        attackSpeed = 1.2f; // Faster attack speed for quick slashes
    }

    protected override void OnAttack()
    {
        // Custom attack logic for Katana
        Debug.Log("Katana Slash! Dealing " + damage + " damage.");

        // Trigger a slight forward dash effect during the attack
        DashForward();

        // Optional: Instantiate a slash particle effect
        if (particleEffect)
        {
            Instantiate(particleEffect, transform.position + transform.forward * range, Quaternion.identity);
        }
    }

    private void DashForward()
    {
        // Move the player forward slightly to simulate a dash
        transform.position += transform.forward * dashDistance;
    }
}

