using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private float[] attackDetails = new float[2];
    Rigidbody2D rb;
    private BoxCollider2D bc;
    bool hasHit;
    private Bow bow;
    private Inventory inventory;
    private bool isMiss;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        bow = GameObject.FindGameObjectWithTag("Player").GetComponent<Bow>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        attackDetails[0] = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rope")
        {
            Destroy(collision.collider.gameObject);
            inventory.arrowSlot++;
            Destroy(gameObject);
        }
        if (collision.collider.tag == "Enemy" && !isMiss)
        {
            attackDetails[1] = transform.position.x;
            
            inventory.arrowSlot++;
            
            collision.collider.transform.parent.SendMessage("Damage", attackDetails);
          
            Destroy(gameObject);  
            
        }
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        isMiss = true;
  
    }
}
