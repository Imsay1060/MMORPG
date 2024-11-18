using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShortSword : Sword
{
    protected override void Start()
    {
        base.Start();
        damage = 8f; // Lower damage
        range = 1.5f; // Shorter range
        attackSpeed = 1.5f; // Faster attacks
    }

    protected override void OnAttack()
    {
        // Custom attack logic for ShortSword
        Debug.Log("ShortSword Attack! Quick strike dealing " + damage + " damage.");
        
        // Add particle effect or quick slash effect here
        if (particleEffect)
        {
            Instantiate(particleEffect, transform.position + transform.forward * range, Quaternion.identity);
        }
    }
}
