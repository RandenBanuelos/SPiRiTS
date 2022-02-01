using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Create New Weapon")]
public class Weapon : Item
{
    [SerializeField] int baseDamage = 0;
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
