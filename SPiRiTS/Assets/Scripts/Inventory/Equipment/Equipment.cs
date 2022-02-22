using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Items that are equipment that can be used by players
/// </summary>
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    /// <summary>
    /// Stores the slot that this equipment will take up
    /// </summary>
    [SerializeField] private EquipmentSlot equipSlot;

    /// <summary>
    /// Int value for the armor modifier
    /// </summary>
    [SerializeField] private int armorModifier;

    /// <summary>
    /// Stores the int value for the damage modifier
    /// </summary>
    [SerializeField] private int damageModifier;


    public EquipmentSlot EquipSlot => equipSlot;

    public override void Use()
    {
        base.Use();

        // Equip the item
        EquipmentManager.Instance.Equip(this);

        // Remove it from the inventory
    }
}


public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Weapon,
    Shield
}
