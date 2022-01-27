using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Create New Healing Item")]
public class HealingItem : Item
{
    [Header("Healing")]
    [SerializeField] private int healthRestored = 0;
    [SerializeField] private bool healsMax = false;


    public int HealthRestored => healthRestored;
    public bool HealsMax => healsMax;
}
