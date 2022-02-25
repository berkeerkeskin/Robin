using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    //gameobject slots
    public int arrowSlot;
    public Text arrowDisplay;
    private void Start()
    {
        arrowSlot = 10;
    }

    private void Update()
    {
        arrowDisplay.text = arrowSlot.ToString();
    }

}
