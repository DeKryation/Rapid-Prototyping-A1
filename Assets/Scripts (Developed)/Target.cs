using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Target : MonoBehaviour
{
    public float health = 50f;
    
    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Death();
        }
    }
    void Death ()
    {
        Destroy(gameObject);
    }
}
