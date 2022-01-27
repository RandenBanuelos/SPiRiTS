using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    [SerializeField] private EquipmentSlot equipSlot;

    [SerializeField] private int armorModifier;
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
