using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    //gameobject slots
    public int arrowSlot;
    private void Start()
    {
        arrowSlot = 10;
    }
    
}
