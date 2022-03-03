using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Documentation by: Matthew Jung

/// <summary>
/// Class for storing item drop data
/// </summary>
[System.Serializable]
public class DroppedItem
{
    // VARIABLES
    /// <summary>
    /// The item that is being dropped
    /// </summary>
    [SerializeField] Item drop;

    /// <summary>
    /// The drop rate of the item
    /// </summary>
    [SerializeField] float dropRate = 0f;


    public Item Drop => drop;

    public float DropRate => dropRate;
}