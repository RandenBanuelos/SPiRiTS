using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DroppedItem
{
    // VARIABLES

    [SerializeField] Item drop;
    [SerializeField] float dropRate = 0f;


    public Item Drop => drop;

    public float DropRate => dropRate;
}
