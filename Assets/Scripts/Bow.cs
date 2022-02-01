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


    // Start is called before the first frame update
    void Start()
    {   
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();

        }
    }

    private void Shoot()
    {
        Debug.Log(inventory.arrowSlot);
        if (inventory.arrowSlot != 0)
        { 
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
            inventory.arrowSlot--;
        } 
    }
}
