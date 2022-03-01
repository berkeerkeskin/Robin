using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    public Arrow arrow;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {       
            if (inventory.arrowSlot != 10)
            {
                //item can be added to the inventory
                inventory.arrowSlot++;
                //Instantiate(itemButton, inventory.slots[i].transform, false);
                Destroy(gameObject);  
                    
                    
            }
            
        }

    }
}
