using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Arrow Variables")]
    public GameObject arrow;
    public float launchForce;
    public Transform shotPoint;
    private Inventory inventory;
    private float holdDownStartTime;
    private float characterFaceDirection;



    // Start is called before the first frame update
    void Start()
    {   
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        characterFaceDirection = GameObject.FindGameObjectWithTag("Player").transform.localScale.x;
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;
        
        bool checkFaceDirection = false; // Direction check
        bool checkMouseDirection = false;
        if (characterFaceDirection >= 1f)
            checkFaceDirection = true;
        if (direction.x >= 1)
            checkMouseDirection = true;

        if (!(checkFaceDirection ^ checkMouseDirection)) //Only if they are in same direction
        {
            if (Input.GetMouseButtonDown(0)) //For launchForce calculation
            {
                        holdDownStartTime = Time.time;
            }
                    
            if (Input.GetMouseButtonUp(0))
            {
                        float holdDownTime = Time.time - holdDownStartTime;
                        //Pass the force result
                        Shoot(CalculateShootForce(holdDownTime));
            
            }
        }
        
    }

    private void Shoot(float shootForce)
    {
        if (inventory.arrowSlot != 0)
        { 
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * shootForce;
            
            inventory.arrowSlot--;

          
        } 
        
    }

    private float CalculateShootForce(float holdTime)
    {
        float maxForceHoldDownTime = 1.5f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDownTime);
        float force = holdTimeNormalized * launchForce;
        if (holdTime <= 0.4f)
            force = 6;
        Debug.Log(force);
        return force;
    }
}
