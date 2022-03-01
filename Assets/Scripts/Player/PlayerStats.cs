using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] 
    private float maxHealth;

    [SerializeField] private GameObject
        deathChunkParticle,
        deathBloodParticle;

    public bool isDead;
    private float currentHealth;

    private Animator animator;
    private void Start()
    {
        currentHealth = maxHealth;
        animator = gameObject.GetComponent<Animator>();
        isDead = false;
    }

    private void Update()
    {
        animator.SetBool("IsDead", isDead);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0.0f)
        {
            isDead = true;
            //Physics2D.IgnoreLayerCollision(7, 8);
            //gameObject.layer = 10;
            Die();
        }
    }

    private void Die()
    {
        //Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        //Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject, 4.0f);  
    }
}
