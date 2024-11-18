using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongSword : Sword
{
    protected override void Start()
    {
        base.Start();
        damage = 15f; // Higher damage for LongSword
        range = 2.5f; // Longer range
        attackSpeed = 0.8f; // Slower than default
    }

    protected override void OnAttack()
    {
        // Custom attack logic for LongSword
        Debug.Log("LongSword Attack! Dealing " + damage + " damage.");
        
        // Add particle effect or damage logic here, e.g., raycasting within range
        if (particleEffect)
        {
            Instantiate(particleEffect, transform.position + transform.forward * range, Quaternion.identity);
        }
    }
}
