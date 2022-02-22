using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing modifier data
/// </summary>
[System.Serializable]
public class Modifier
{
    /// <summary>
    /// The element for the modifier
    /// </summary>
    [SerializeField] private ElementType element = ElementType.None;

    /// <summary>
    /// The modifier value
    /// </summary>
    [SerializeField] private float mod = 1f;


    public ElementType Element => element;

    public float Mod => mod;
}
