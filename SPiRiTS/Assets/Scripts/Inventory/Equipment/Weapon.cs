using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Documentation by: Matthew Jung

/// <summary>
/// Items that are weapons that can be used by players/enemies
/// </summary>
[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Create New Weapon")]
public class Weapon : Item
{
    /// <summary>
    /// The damage the weapon deals (before weaknesses and resistances)
    /// </summary>
    [SerializeField] int baseDamage = 0;

    /// <summary>
    /// The element type of the weapon
    /// </summary>
    [SerializeField] ElementType element = ElementType.None;


    public int BaseDamage => baseDamage;
    public ElementType Element => element;
}


public enum ElementType
{
    None,
    Water,
    Fire,
    Electric,
    Earth,
    Frost,
    Storm,
    Shadow,
    Life
}