using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
     
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Damage");

        if (currentHealth <= 0.0f)
        {
            animator.SetTrigger("Die");
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
        gameObject.layer = LayerMask.NameToLayer("Dead"); //untouchable
        Destroy(gameObject, 5.0f);  
    }
}
